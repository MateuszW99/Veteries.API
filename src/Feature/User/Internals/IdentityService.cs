using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Helpers;
using Persistence.Domain;
using User.Abstractions;
using IdentityResult = User.Models.Results.IdentityResult;

namespace User.Internals
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly AppSettings _appSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly DomainDbContext _context;

        public IdentityService(UserManager<ApplicationUser> userManager, 
            ITokenService tokenService,
            IOptions<AppSettings> appSettings,
            TokenValidationParameters tokenValidationParameters,
            DomainDbContext context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _appSettings = appSettings.Value;
            _tokenValidationParameters = tokenValidationParameters;
            _context = context;
        }


        public async Task<IdentityResult> RegisterUser(string email)
        {
            var checkoutUserEmail = await _userManager.FindByEmailAsync(email);

            // the user already exists
            if (checkoutUserEmail != null)
            {
                return IdentityResult.UserAlreadyExistsResult();
            };

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            // create new user
            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                return IdentityResult.ErrorWhenCreatingUserResult();
            }

            return await GenerateAuthenticationResultAsync(user);
        }

        public async Task<IdentityResult> AuthenticateUser(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return IdentityResult.UserDoesNotExistResult();
            }

            var passwordValidator = new PasswordValidator<ApplicationUser>();
            var isPasswordValid = await passwordValidator.ValidateAsync(_userManager, user, password);

            if (!isPasswordValid.Succeeded)
            {
                return IdentityResult.InvalidPasswordResult();
            }

            //var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            //if (!userHasValidPassword)
            //{
            //   return IdentityResult.InvalidPasswordResult();
            //}

            return await GenerateAuthenticationResultAsync(user);
        }

        public async Task<IdentityResult> RefreshToken(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
            {
                return IdentityResult.TokenDoesNotExistResult();
            }

            var expiryDateUnix = long.Parse(validatedToken.Claims
                .Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix)
                .Subtract(_appSettings.TokenLifetime);

            if (expiryDateUtc > DateTime.UtcNow)
            {
                return IdentityResult.TokenHasNotExpiredResult();
            }

            
            var tokenId = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken =
                await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

            if (storedRefreshToken == null)
            {
                return IdentityResult.TokenDoesNotExistResult();
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return IdentityResult.TokenHasExpiredResult();
            }

            if (storedRefreshToken.Invalidated)
            {
                return IdentityResult.InvalidatedTokenResult();
            }

            if (storedRefreshToken.Used)
            {
                return IdentityResult.TokenAlreadyUsedResult();
            }

            if (storedRefreshToken.JwtId != tokenId)
            {
                return IdentityResult.TokensDoNotMatchResult();
            }

            storedRefreshToken.Used = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims
                .Single(x => x.Type == "id").Value);

            return await GenerateAuthenticationResultAsync(user);
        }

        private async Task<IdentityResult> GenerateAuthenticationResultAsync(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id)
                }),
                Expires = DateTime.UtcNow.Add(_appSettings.TokenLifetime),
                Issuer = _appSettings.ValidIssuer,
                Audience = _appSettings.ValidAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            RefreshToken refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new IdentityResult()
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal =
                    tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}


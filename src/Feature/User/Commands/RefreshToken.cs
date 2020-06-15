using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Helpers;
using Persistence.Domain;
using User.Abstractions;

namespace User.Commands
{
    public class RefreshToken
    {
        public class Command : IRequest<Result>
        {
            public string Token { get; set; }
            public string RefreshToken { get; set; }
        }

        public class Result
        {
            public string Token { get; set; }
            public string RefreshToken { get; set; }
            public bool Success { get; set; }
            public IEnumerable<string> ErrorMessages { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                this.RuleFor(x => x.Token)
                    .NotEmpty()
                    .NotEqual(xx => xx.RefreshToken);

                this.RuleFor(x => x.RefreshToken)
                    .NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly ITokenService _tokenService;
            private readonly AppSettings _appSettings;
            private readonly TokenValidationParameters _tokenValidationParameters;
            private readonly DomainDbContext _context;
            private readonly UserManager<ApplicationUser> _userManager;

            public Handler(ITokenService tokenService,
                IOptions<AppSettings> appSettings,
                DomainDbContext context,
                UserManager<ApplicationUser> userManager, TokenValidationParameters tokenValidationParameters)
            {
                _tokenService = tokenService;
                _appSettings = appSettings.Value;
                _context = context;
                _userManager = userManager;
                _tokenValidationParameters = tokenValidationParameters;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var validatedToken = GetPrincipalFromToken(request.Token);

                if (validatedToken == null)
                {
                    return new Result()
                    {
                        Success = false,
                        ErrorMessages = new[] {"Invalid Token"}
                    };
                }

                var expiryDateUnix = long.Parse(validatedToken.Claims
                    .Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(expiryDateUnix)
                    .Subtract(_appSettings.TokenLifetime);

                if (expiryDateUtc > DateTime.UtcNow)
                {
                    return new Result()
                    {
                        Success = false,
                        ErrorMessages = new[] { "This Token hasn't expired yet" }
                    };
                }

                // token id
                var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == request.RefreshToken);

                if (storedRefreshToken == null)
                {
                    return new Result()
                    {
                        Success = false,
                        ErrorMessages = new[] { "This Token doesn't exist" }
                    };
                }

                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                {
                    return new Result()
                    {
                        Success = false,
                        ErrorMessages = new[] { "This Token has expired" }
                    };
                }

                if (storedRefreshToken.Invalidated)
                {
                    return new Result()
                    {
                        Success = false,
                        ErrorMessages = new[] { "This Token has been invalidated" }
                    };
                }

                if (storedRefreshToken.Used)
                {
                    return new Result()
                    {
                        Success = false,
                        ErrorMessages = new[] { "This Token has been used" }
                    };
                }

                if (storedRefreshToken.JwtId != jti)
                {
                    return new Result()
                    {
                        Success = false,
                        ErrorMessages = new[] { "This Token doesn't match this JWT" }
                    };
                }

                storedRefreshToken.Used = true;
                _context.RefreshTokens.Update(storedRefreshToken);
                await _context.SaveChangesAsync();

                var user = await _userManager.FindByIdAsync(validatedToken.Claims
                    .Single(x => x.Type == "id").Value);

                return await GenerateAuthenticationResultAsync(user);
            }

            private ClaimsPrincipal GetPrincipalFromToken(string token)
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                try
                {
                    var tokenValidationParameters = _tokenValidationParameters.Clone();
                    tokenValidationParameters.ValidateLifetime = false;
                    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
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

            private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
            {
                return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                       jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                           StringComparison.InvariantCultureIgnoreCase);
            }

            private async Task<Result> GenerateAuthenticationResultAsync(ApplicationUser user)
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
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                Domain.Entities.RefreshToken refreshToken = new Domain.Entities.RefreshToken()
                {
                    JwtId = token.Id,
                    UserId = user.Id,
                    CreationDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddMonths(6)
                };

                await _context.RefreshTokens.AddAsync(refreshToken);
                await _context.SaveChangesAsync();

                return new Result
                {
                    Success = true,
                    Token = tokenHandler.WriteToken(token),
                    RefreshToken = refreshToken.Token
                };
            }
        }
    }
}

using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Helpers;
using Persistence.Domain;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using User.Abstractions;

namespace User.Commands
{
    public class AuthenticateUser
    {
        public class Command : IRequest<Result>
        {
            public string Email { get; set; }
            public string Password { get; set; }
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
                this.RuleFor(x => x.Email).NotEmpty()
                    .When(x => x.Email.Equals("") && x.Password.Equals(""));

                this.RuleFor(x => x.Email)
                    .EmailAddress().When(x => !x.Password.Equals(""))
                    .NotEmpty().When(x => !x.Password.Equals(""));

                this.RuleFor(x => x.Password)
                    .NotEmpty().When(x => !x.Email.Equals(""));
            }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly ITokenService _tokenService;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly AppSettings _appSettings;
            private readonly DomainDbContext _dbContext;

            public Handler(ITokenService tokenService, SignInManager<ApplicationUser> signInManager,
                UserManager<ApplicationUser> userManager,
                IOptions<AppSettings> appSettings,
                DomainDbContext dbContext)
            {
                _dbContext = dbContext;
                _tokenService = tokenService;
                _signInManager = signInManager;
                _userManager = userManager;
                _appSettings = appSettings.Value;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                ApplicationUser user = null;
                if (!request.Equals("") && !request.Password.Equals(""))
                {
                    var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, true, false);
                    if (result.Succeeded)
                    {
                        user = await _userManager.FindByEmailAsync(request.Email);
                    }
                }
                else
                {
                    var result = await _dbContext.RefreshTokens
                        .FirstOrDefaultAsync(x => x.RefreshToken == request.RefreshToken && x.Expires.CompareTo(DateTime.UtcNow) >= 0);
                    if (result != null)
                    {
                        user = await _userManager.FindByIdAsync(result.UserId.ToString());
                    }
                }

                if (user != null)
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
                    var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var roles = await _userManager.GetRolesAsync(user);
                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    var tokenOptions = new JwtSecurityToken(
                        issuer: _appSettings.ValidIssuer,
                        audience: _appSettings.ValidAudience,
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: signInCredentials
                        );
                    var refreshToken = _tokenService.GenerateToken(32);
                    _dbContext.RefreshTokens.Add(new Token
                    {
                        RefreshToken = refreshToken,
                        Expires = DateTime.UtcNow.AddDays(5),
                        UserId = user.Id,
                        RemoteIpAddress = ""
                    });
                    await _dbContext.SaveChangesAsync();

                    return new Result
                    {
                        Success = true,
                        Token = new JwtSecurityTokenHandler().WriteToken(tokenOptions),
                        RefreshToken = refreshToken
                    };
                }

                return new Result
                {
                    Success = false,
                    ErrorMessages = new[] { "Unauthorized" }
                };
            }
        }
    }
}

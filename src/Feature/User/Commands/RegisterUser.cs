using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using User.Abstractions;
using User.Models;

namespace User.Commands
{
    public class RegisterUser
    {
        public class Command : IRequest<Result>
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string RepeatedPassword { get; set; }
        }

        public class Result
        {
            public string Token { get; set; }
            public bool Success { get; set; }
            public IEnumerable<string> ErrorMessages { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                this.RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress();

                this.RuleFor(x => x.Password)
                    .Length(8, 24)
                    .NotEmpty()
                    .Equal(x => x.RepeatedPassword);
            }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly ITokenService _tokenService;
            private readonly AppSettings _appSettings;

            public Handler(UserManager<ApplicationUser> userManager,
                ITokenService tokenService,
                IOptions<AppSettings> appSettings)
            {
                _userManager = userManager;
                _tokenService = tokenService;
                _appSettings = appSettings.Value;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var checkoutUserEmail = await _userManager.FindByEmailAsync(request.Email);

                // the user already exists
                if (checkoutUserEmail != null)
                {
                    return new Result
                    {
                        Success = false,
                        ErrorMessages = new[] { "This email address is already registered" }
                    };
                };

                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email
                };

                // create new user
                var result = await _userManager.CreateAsync(user);

                if (result.Errors.Any())
                {
                    return new Result
                    {
                        Success = false,
                        ErrorMessages = result.Errors.Select(x => x.ToString())
                    };
                }

                return GenerateAuthenticationResult(user);
            }

            private Result GenerateAuthenticationResult(ApplicationUser user)
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

                return new Result
                {
                    Success = true,
                    Token = tokenHandler.WriteToken(token)
                };
            }
        }
    }
}


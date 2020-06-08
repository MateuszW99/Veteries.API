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
using System.Linq;
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
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly AppSettings _appSettings;
            private readonly DomainDbContext _dbContext;

            public Handler(ITokenService tokenService,
                UserManager<ApplicationUser> userManager,
                IOptions<AppSettings> appSettings,
                DomainDbContext dbContext)
            {
                _dbContext = dbContext;
                _tokenService = tokenService;
                _userManager = userManager;
                _appSettings = appSettings.Value;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    return new Result
                    {
                        Success = false,
                        ErrorMessages = new[] { "User does not exist" }
                    };
                }

                //var userHasValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

                //if (!userHasValidPassword)
                //{
                //    return new Result
                //    {
                //        Success = false,
                //        ErrorMessages = new[] { "User/password combination is wrong" }
                //    };
                //}

                return await GenerateAuthenticationResultAsync(user);
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
                    Expires = DateTime.UtcNow.AddHours(1),
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

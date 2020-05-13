using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace User.Commands
{
    public class RegisterUser
    {
        public class Command : IRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string RepeatedPassword { get; set; }
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

        public class Handler : IRequestHandler<Command>
        {
            private readonly UserManager<ApplicationUser> userManager;

            public Handler(UserManager<ApplicationUser> userManager)
            {
                this.userManager = userManager;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var checkoutUserEmail = await userManager.FindByEmailAsync(request.Email);
                if (checkoutUserEmail != null)
                {
                    throw new Exception("This email address is already registered");
                }
                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email
                };
                var result = await userManager.CreateAsync(user);
                if (result.Errors.Any())
                {
                    throw new Exception(result.Errors.FirstOrDefault().Description);
                }
                return Unit.Value;
            }
        }
    }
}

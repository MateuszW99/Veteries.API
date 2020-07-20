using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using User.Abstractions;
using User.Models.Commands;
using IdentityResult = User.Models.Results.IdentityResult;

namespace User.Commands
{
    public class RegisterUserHandler
    {
        public class Validator : AbstractValidator<RegisterUserCommand>
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

        public class Handler : IRequestHandler<RegisterUserCommand, IdentityResult>
        {
            private readonly IIdentityService _identityService;

            public Handler(IIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                if (request.IsNull())
                {
                    return IdentityResult.EmptyRequestResult();
                }

                return await _identityService.RegisterUser(request.Email);
            }
        }
    }
}


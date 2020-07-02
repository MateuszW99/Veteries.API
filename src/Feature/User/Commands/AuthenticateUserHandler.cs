using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using User.Abstractions;
using User.Models.Commands;
using IdentityResult = User.Models.Results.IdentityResult;

namespace User.Commands
{
    public class AuthenticateUserHandler
    {
        public class Validator : AbstractValidator<AuthenticateUserCommand>
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

        public class Handler : IRequestHandler<AuthenticateUserCommand, IdentityResult>
        {
            private readonly IIdentityService _identityService;

            public Handler(IIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task<IdentityResult> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
            {
                if (request.IsNull())
                {
                    return IdentityResult.EmptyRequestResult();
                }

                return await _identityService.AuthenticateUser(request.Email, request.Password);
            }
        }
    }
}

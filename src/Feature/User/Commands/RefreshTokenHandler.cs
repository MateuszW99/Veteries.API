using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using User.Abstractions;
using User.Models.Commands;
using IdentityResult = User.Models.Results.IdentityResult;

namespace User.Commands
{
    public class RefreshTokenHandler
    {
        public class Validator : AbstractValidator<RefreshTokenCommand>
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

        public class Handler : IRequestHandler<RefreshTokenCommand, IdentityResult>
        {
            private readonly IIdentityService _identityService;

            public Handler(IIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task<IdentityResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
            {
                if (request.IsNull())
                {
                    return IdentityResult.EmptyRequestResult();
                }

                return await _identityService.RefreshToken(request.Token, request.RefreshToken);
            }
        }
    }
}

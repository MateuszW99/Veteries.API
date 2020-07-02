using MediatR;
using User.Models.Results;

namespace User.Models.Commands
{
    public class AuthenticateUserCommand : IdentityCommand, IRequest<IdentityResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

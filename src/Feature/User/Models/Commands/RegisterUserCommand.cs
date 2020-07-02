using MediatR;
using User.Models.Results;

namespace User.Models.Commands
{
    public class RegisterUserCommand : IdentityCommand, IRequest<IdentityResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatedPassword { get; set; }
    }
}

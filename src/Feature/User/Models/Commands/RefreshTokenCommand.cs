using MediatR;
using User.Models.Results;

namespace User.Models.Commands
{
    public class RefreshTokenCommand : IdentityCommand, IRequest<IdentityResult>
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}

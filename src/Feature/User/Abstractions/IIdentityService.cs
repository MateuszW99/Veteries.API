using System.Threading.Tasks;
using User.Models.Results;

namespace User.Abstractions
{
    public interface IIdentityService
    {
        Task<IdentityResult> RegisterUser(string email);
        Task<IdentityResult> AuthenticateUser(string email, string password);
        Task<IdentityResult> RefreshToken(string token, string refreshToken);
    }
}

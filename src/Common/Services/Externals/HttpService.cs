using System.Linq;
using Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Services.Externals
{
    public class HttpService : IHttpService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            if (_httpContextAccessor?.HttpContext.User == null)
            {
                return string.Empty;
            }

            return _httpContextAccessor.HttpContext.User.Claims
                .Single(x => x.Type == JwtRegisteredClaimNames.NameId).Value;
        }
    }
}
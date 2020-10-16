using System.Linq;
using System.Security.Claims;
using Services.Abstractions;
using Microsoft.AspNetCore.Http;

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
                .Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
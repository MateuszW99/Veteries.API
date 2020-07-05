using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Extensions
{
    public static class HttpContextExtension
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            return httpContext.User == null ? string.Empty : httpContext.User.Claims.Single(x => x.Type == "id").Value;
        }
    }
}

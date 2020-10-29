using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Extensions
{
    public static class HttpContextExtension
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return string.Empty;
            }
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            //var x = httpContext;
            //var ss = httpContext.User.Identity.Name;
            var t = httpContext.User.Claims.ToString();
            return httpContext.User.Claims.Single(x => x.Type == "id").Value.ToString();
        }
    }
}

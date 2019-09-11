using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using Bedrock.Shared.Security.Identity;
using Bedrock.Shared.Security.ResourceAuthorization;

using Microsoft.AspNetCore.Http;

namespace Bedrock.Shared.Web.Extension
{
    public static class HttpContextExtension
    {
        #region Public Methods
        public static string GetUsername(this HttpContext httpContext)
        {
            var windowsIdentity = WindowsIdentity.GetCurrent();
            var userName = windowsIdentity.GetLogin();
            var contextName = httpContext?.User?.Identity?.GetLogin();

            return !string.IsNullOrWhiteSpace(contextName) ? contextName : userName;
        }

        public static Task<bool> CheckAccessAsync(this HttpContext httpContext, string resource, string[] permissions, string resourceType = null)
        {
            var claimsPrincipal = httpContext.User as ClaimsPrincipal;
            var authorizationContext = new ResourceAuthorizationContext
            (
                resource,
                permissions,
                httpContext.User ?? Principal.Anonymous,
                resourceType
            );

            return httpContext.CheckAccessAsync(authorizationContext);
        }

        public static Task<bool> CheckAccessAsync(this HttpContext httpContext, ResourceAuthorizationContext authorizationContext)
        {
            return HttpActionContextExtension.GetResourceAuthorizationManager(httpContext).CheckAccessAsync(authorizationContext);
        }
        #endregion
    }
}

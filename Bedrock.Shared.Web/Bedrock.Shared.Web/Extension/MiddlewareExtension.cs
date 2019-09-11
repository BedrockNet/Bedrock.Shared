using Bedrock.Shared.Security.Interface;
using Bedrock.Shared.Security.ResourceAuthorization;

using Bedrock.Shared.Web.Middleware;
using Bedrock.Shared.Web.Middleware.Options;

using Microsoft.AspNetCore.Builder;

namespace Bedrock.Shared.Web.Extension
{
    public static class MiddlewareExtension
    {
        #region Public Methods
        public static IApplicationBuilder UseResourceAuthorization(this IApplicationBuilder application, IResourceAuthorizationManager authorizationManager)
        {
            var options = new ResourceAuthorizationMiddlewareOptions
            {
                Manager = authorizationManager
            };

            return application.UseResourceAuthorization(options);
        }

        public static IApplicationBuilder UseResourceAuthorization(this IApplicationBuilder application, ResourceAuthorizationMiddlewareOptions options)
        {
            return application.UseMiddleware<ResourceAuthorizationManagerMiddleware>(options);
        }

        public static IApplicationBuilder UseTestUserMiddleware(this IApplicationBuilder application, params string[] applications)
        {
            var options = new TestUserMiddlewareOptions(applications);
            return application.UseTestUserMiddleware(options);
        }

        public static IApplicationBuilder UseTestUserMiddleware(this IApplicationBuilder application, TestUserMiddlewareOptions options)
        {
            return application.UseMiddleware<TestUserMiddleware>(options);
        }

        public static IApplicationBuilder UseApplicationException(this IApplicationBuilder application, string errorMessage = null)
        {
            var options = new ApplicationExceptionMiddlewareOptions(errorMessage);
            return application.UseMiddleware<ApplicationExceptionMiddleware>(options);
        }

        public static IApplicationBuilder UseHttpException(this IApplicationBuilder application)
        {
            return application.UseMiddleware<HttpExceptionMiddleware>();
        }

        public static IApplicationBuilder UseEndRequest(this IApplicationBuilder application)
        {
            return application.UseMiddleware<EndRequestMiddleware>();
        }

        public static IApplicationBuilder UsePostAuthenticateMiddleware(this IApplicationBuilder application, IClaimType claimType)
        {
            var options = new PostAuthenticationMiddlewareOptions(claimType);
            return application.UsePostAuthenticateMiddleware(options);
        }

        public static IApplicationBuilder UsePostAuthenticateMiddleware(this IApplicationBuilder application, PostAuthenticationMiddlewareOptions options)
        {
            return application.UseMiddleware<PostAuthenticateMiddleware>(options);
        }
        #endregion
    }
}

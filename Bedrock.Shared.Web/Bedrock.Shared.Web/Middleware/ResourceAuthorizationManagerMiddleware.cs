using System.Threading.Tasks;
using Bedrock.Shared.Security.ResourceAuthorization;
using Microsoft.AspNetCore.Http;

namespace Bedrock.Shared.Web.Middleware
{
    public class ResourceAuthorizationManagerMiddleware
    {
        #region Fields
        private readonly RequestDelegate _next;
        private ResourceAuthorizationMiddlewareOptions _options;

        public const string Key = "idm:resourceAuthorizationManager";
        #endregion

        #region Constructors
        public ResourceAuthorizationManagerMiddleware(RequestDelegate next, ResourceAuthorizationMiddlewareOptions options)
        {
            _options = options;
            _next = next;
        }
        #endregion

        #region Public Methods
        public async Task Invoke(HttpContext context)
        {
            context.Items[Key] = _options.Manager ?? _options.ManagerProvider(context.Items);
            await _next.Invoke(context);
        }
        #endregion
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Bedrock.Shared.Enumeration.StringHelper;

using Bedrock.Shared.Security.Identity;
using Bedrock.Shared.Security.Interface;
using Bedrock.Shared.Security.ResourceAuthorization;

using Bedrock.Shared.Utility;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bedrock.Shared.Web.Extension
{
    public static class HttpActionContextExtension
    {
        #region Public Methods (Extension)
        public static Task<bool> CheckAccessAsync(this AuthorizationFilterContext context, string resource, string[] permissions, string resourceType = null)
        {
            var authorizationContext = new ResourceAuthorizationContext
            (
                resource,
                permissions,
                context.HttpContext.User ?? Principal.Anonymous,
                resourceType
            );

            return context.CheckAccessAsync(authorizationContext);
        }

        public static Task<bool> CheckAccessAsync(this AuthorizationFilterContext context, List<Claim> claims)
        {
            var authorizationContext = new ResourceAuthorizationContext
            (
                claims,
                context.HttpContext.User ?? Principal.Anonymous
            );

            return context.CheckAccessAsync(authorizationContext);
        }

        public static Task<bool> CheckAccessAsync(this AuthorizationFilterContext context, ResourceAuthorizationContext authorizationContext)
        {
            return GetResourceAuthorizationManager(context).CheckAccessAsync(authorizationContext);
        }

        public static IEnumerable<Claim> ResourcesFromRouteParameters(this AuthorizationFilterContext context)
        {
            return context.RouteData.Values.Select(arg => new Claim(arg.Key, arg.Value.ToString()));
        }

        public static List<Claim> ResourceFromController(this AuthorizationFilterContext context)
        {
            var controller = StringHelper.Current.Lookup(StringApplication.Controller);
            return new List<Claim> { new Claim(controller, context.RouteData.Values[controller].ToString()) };
        }

        public static Claim ActionFromController(this AuthorizationFilterContext context)
        {
            var action = StringHelper.Current.Lookup(StringApplication.Action);
            return new Claim(action, context.RouteData.Values[action].ToString());
        }
        #endregion

        #region Public Methods
        public static IResourceAuthorizationManager GetResourceAuthorizationManager(AuthorizationFilterContext request)
        {
            return GetResourceAuthorizationManager(request.HttpContext);
        }

        public static IResourceAuthorizationManager GetResourceAuthorizationManager(HttpContext context)
        {
            var service = context.RequestServices.GetService(typeof(IResourceAuthorizationManager));
            var resourceAuthorizationManager = service as IResourceAuthorizationManager;

            if (resourceAuthorizationManager == null)
                throw new ArgumentNullException(nameof(resourceAuthorizationManager), StringHelper.Current.Lookup(StringSecurity.NoAuthorizationManagerSet));

            return resourceAuthorizationManager;
        }
        #endregion
    }
}

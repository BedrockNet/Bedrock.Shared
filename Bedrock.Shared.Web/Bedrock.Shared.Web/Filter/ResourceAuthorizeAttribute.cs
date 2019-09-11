using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Bedrock.Shared.Configuration;
using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Extension;
using Bedrock.Shared.Security.Constant;
using Bedrock.Shared.Utility;
using Bedrock.Shared.Web.Extension;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bedrock.Shared.Web.Filter
{
    public class ResourceAuthorizeAttribute : TypeFilterAttribute
    {
		#region Constructors
		public ResourceAuthorizeAttribute(string resource, string permission, string resourceType = null) : this(resource, new string[] { permission }, resourceType) { }

        public ResourceAuthorizeAttribute(string resource, string[] permissions, string resourceType = null) : base(typeof(ResourceAuthorizeFilter))
        {
            var claims = new List<Claim>();
            var permissionClaimType = StringHelper.Current.Lookup(StringClaimType.Permission);
            var resourceTypeProperty = StringHelper.Current.Lookup(StringSecurity.ResourceType);

            permissions.Each(p =>
            {
                var claim = new Claim(permissionClaimType, p, resource, Issuer.BedrockSharedSecurity, Issuer.BedrockSharedSecurity);

                if(!string.IsNullOrEmpty(resourceType))
                    claim.Properties[resourceTypeProperty] = resourceType;

                claims.Add(claim);
            });

            Arguments = new object[] { claims };
        }
        #endregion
    }

    public class ResourceAuthorizeFilter : IAuthorizationFilter
    {
        #region Constructors
        public ResourceAuthorizeFilter(List<Claim> claims)
        {
            Claims = claims;
        }
        #endregion

        #region Protected Properties
        protected List<Claim> Claims { get; set; }
        #endregion

        #region IAuthorizationFilter Members
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAuthorized = EnsureAuthorized(context);

            if (!isAuthorized)
            {

                if (context.HttpContext.User != null && context.HttpContext.User.Identity != null && context.HttpContext.User.Identity.IsAuthenticated)
                    context.Result = new ForbidResult();
                else
                    context.Result = new UnauthorizedResult();
            }
        }
        #endregion

        #region Protected Methods
        protected virtual bool CheckAccess(AuthorizationFilterContext context, List<Claim> claims)
        {
            return AsyncHelper.RunSync(() => CheckAccessAsync(context, claims));
        }
        #endregion

        #region Private Methods
        private bool EnsureAuthorized(AuthorizationFilterContext context)
        {
            var returnValue = false;
            returnValue = CheckAccess(context, Claims);
            return returnValue;
        }

        private async Task<bool> CheckAccessAsync(AuthorizationFilterContext context, List<Claim> claims)
        {
            var task = context.CheckAccessAsync(claims);
            var bedrockConfiguration = (BedrockConfiguration)context.HttpContext.RequestServices.GetService(typeof(BedrockConfiguration));

            if (await Task.WhenAny(task, Task.Delay(bedrockConfiguration.Security.Application.Authorization.Timeout)).ConfigureAwait(false) == task)
            {
                var result = await task.ConfigureAwait(false);
                return result;
            }
            else
                throw new TimeoutException();
        }
        #endregion
    }
}

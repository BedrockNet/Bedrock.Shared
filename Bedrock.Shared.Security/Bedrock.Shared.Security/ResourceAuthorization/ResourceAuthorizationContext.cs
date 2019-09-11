using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Extension;
using Bedrock.Shared.Security.Interface;
using Bedrock.Shared.Utility;

namespace Bedrock.Shared.Security.ResourceAuthorization
{
    public class ResourceAuthorizationContext : IResourceAuthorizationContext
    {
        #region Constructors
        public ResourceAuthorizationContext(string resource, string[] permissions, ClaimsPrincipal principal, string resourceType = null)
        {
            if (string.IsNullOrEmpty(resource))
                throw new ArgumentNullException(nameof(resource));

            if (permissions == null || !permissions.Any())
                throw new ArgumentNullException(nameof(permissions));

            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var claims = new List<Claim>();
            var permissionClaimType = StringHelper.Current.Lookup(StringClaimType.Permission);
            var resourceTypeProperty = StringHelper.Current.Lookup(StringSecurity.ResourceType);

            permissions.Each(p =>
            {
                var claim = new Claim(permissionClaimType, p, resource);

                if(!string.IsNullOrEmpty(resourceType))
                    claim.Properties[resourceTypeProperty] = resourceType;

                claims.Add(claim);
            });

            Claims = claims;
            Principal = principal;
        }

        public ResourceAuthorizationContext(IEnumerable<Claim> claims, ClaimsPrincipal principal)
        {
            if (claims == null || !claims.Any())
                throw new ArgumentNullException(nameof(claims));

            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            Claims = claims;
            Principal = principal;
        }
        #endregion

        #region Properties
        public IEnumerable<Claim> Claims { get; set; }

        public ClaimsPrincipal Principal { get; set; }
        #endregion
    }
}

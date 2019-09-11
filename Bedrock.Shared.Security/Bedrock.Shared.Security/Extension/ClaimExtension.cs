using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using Bedrock.Shared.Security.Interface;
using Bedrock.Shared.Security.Utility;

namespace Bedrock.Shared.Security.Extension
{
    public static class ClaimExtension
    {
        #region Public Methods
        public static bool HasClaimType(this IEnumerable<Claim> claims, string claimType)
        {
            return claims.Any(c => c.Type == claimType);
        }

        public static Claim GetClaimByTypeFirstOrDefault(this IEnumerable<Claim> claims, string type)
        {
            return claims.FirstOrDefault(c => c.Type == type);
        }

        public static bool HasAccess(this IEnumerable<Claim> userClaims, IResourceAuthorizationContext context)
        {
            return context.Claims.All(c => userClaims.Contains(c, new ClaimComparer()));
        }
        #endregion
    }
}

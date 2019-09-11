using System;
using System.Linq;
using System.Security.Claims;

using Bedrock.Shared.Security.Interface;

namespace Bedrock.Shared.Security.Extension
{
    public static class ClaimsPrincipalExtension
    {
        #region Public Methods
        public static ClaimsPrincipal Transform(this ClaimsPrincipal principal, IClaimType claimType)
        {
            var scopeClaims = principal.FindAll(claimType.ScopeClaimType).ToList();

            if (scopeClaims.Count != 1 || !scopeClaims[0].Value.Contains(' '))
                return principal;

            var claim = scopeClaims[0];
            var scopes = claim.Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var claims = scopes.Select(s => new Claim(claimType.ScopeClaimType, s));

            return new ClaimsPrincipal(new ClaimsIdentity(principal.Identity, claims));
        }
        #endregion
    }
}

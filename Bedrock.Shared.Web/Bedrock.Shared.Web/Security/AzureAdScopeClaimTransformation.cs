using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Bedrock.Shared.Security.Extension;
using Bedrock.Shared.Security.Interface;

using Microsoft.AspNetCore.Authentication;

namespace Bedrock.Shared.Web.Security
{
    /// <summary>
    /// Splits the scope claim assigned by Azure AD
    /// by spaces so that you can check for a scope with
    /// <code>User.HasClaim(Constants.ScopeClaimType, "scope")</code>,
    /// instead of having to split by space every time.
    /// </summary>
    public class AzureAdScopeClaimTransformation : IClaimsTransformation
    {
        #region Fields
        private readonly IClaimType claimType;
        #endregion

        #region Constructors
        public AzureAdScopeClaimTransformation(IClaimType claimType) => this.claimType = claimType;
        #endregion

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            return Task.FromResult(principal.Transform(claimType));
        }
    }
}

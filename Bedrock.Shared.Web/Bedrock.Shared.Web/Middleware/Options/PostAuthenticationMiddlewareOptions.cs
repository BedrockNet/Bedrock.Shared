using Bedrock.Shared.Security.Interface;

namespace Bedrock.Shared.Web.Middleware.Options
{
    public class PostAuthenticationMiddlewareOptions
    {
        public PostAuthenticationMiddlewareOptions(IClaimType claimType)
        {
            ClaimType = claimType;
        }

        #region Properties
        public IClaimType ClaimType { get; set; }
        #endregion
    }
}

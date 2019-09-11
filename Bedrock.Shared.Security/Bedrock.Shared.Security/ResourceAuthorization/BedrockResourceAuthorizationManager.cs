using System.Threading.Tasks;

using Bedrock.Shared.Configuration;

using Bedrock.Shared.Security.Extension;
using Bedrock.Shared.Security.Interface;

namespace Bedrock.Shared.Security.ResourceAuthorization
{
    public class BedrockResourceAuthorizationManager : ResourceAuthorizationManagerBase
    {
        #region Constructors
        public BedrockResourceAuthorizationManager
        (
            IClaimType claimType,
            IClaimCollectorFactory claimCollectorFactory,
            BedrockConfiguration bedrockConfiguration
        ) : base
        (
            claimType,
            claimCollectorFactory,
            bedrockConfiguration
        ) { }
        #endregion

        #region Public Methods
        public override Task<bool> CheckAccessAsync(IResourceAuthorizationContext context)
        {
            AddPermissionClaims(context);
            return Eval(context.Principal.Claims.HasAccess(context));
        }
        #endregion
    }
}

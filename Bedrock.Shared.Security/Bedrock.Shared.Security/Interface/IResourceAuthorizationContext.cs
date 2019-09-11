using System.Collections.Generic;
using System.Security.Claims;

namespace Bedrock.Shared.Security.Interface
{
    public interface IResourceAuthorizationContext
    {
        #region Properties
        IEnumerable<Claim> Claims { get; set; }

        ClaimsPrincipal Principal { get; set; }
        #endregion
    }
}

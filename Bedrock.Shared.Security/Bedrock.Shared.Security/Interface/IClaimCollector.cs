using System.Collections.Generic;
using System.Security.Claims;

namespace Bedrock.Shared.Security.Interface
{
    public interface IClaimCollector
    {
        #region Methods
        IEnumerable<Claim> Collect(ICollectPass collectPass);
        #endregion
    }
}

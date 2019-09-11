using Bedrock.Shared.Cache.Interface;

namespace Bedrock.Shared.Security.Interface
{
    public interface ICollectPass
    {
        #region Properties
        string CollectPassName { get; set; }

        IClaimCollector Collector { get; set; }

        string Issuer { get; set; }

        string Application { get; set; }

        ICacheProvider Cache { get; set; }

        string Username { get; set; }

        string SubjectId { get; set; }
        #endregion

        #region Methods
        void Validate();
        #endregion
    }
}

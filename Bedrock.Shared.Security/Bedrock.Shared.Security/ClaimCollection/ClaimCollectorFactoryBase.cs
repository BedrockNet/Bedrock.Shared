using Bedrock.Shared.Cache.Interface;
using Bedrock.Shared.Configuration;
using Bedrock.Shared.Security.Interface;

namespace Bedrock.Shared.Security.ClaimCollection
{
    public abstract class ClaimCollectorFactoryBase : IClaimCollectorFactory
    {
        #region Constructors
        public ClaimCollectorFactoryBase(ICacheProvider cache, BedrockConfiguration bedrockConfiguration)
        {
            BedrockConfiguration = bedrockConfiguration;

            Cache = cache;
            Cache.SetCacheType(BedrockConfiguration.Security.Application.ClaimCollection.CacheType);
        }
        #endregion

        #region Protected Properties
        protected ICacheProvider Cache { get; set; }

        protected BedrockConfiguration BedrockConfiguration { get; set; }
        #endregion

        #region IAFClaimCollectorFactory Methods
        public abstract IClaimCollector CreateInstanceCollector(string application);

        public abstract ICollectPass CreateInstancePass(string application, IClaimCollector collector, string userId, string subjectId);
        #endregion

        #region Protected Methods
        protected virtual string GetConnectionString(string application)
        {
            return string.Format(BedrockConfiguration.Security.Application.ClaimCollection.ConnectionKey, application.ToString());
        }
    }
    #endregion
}

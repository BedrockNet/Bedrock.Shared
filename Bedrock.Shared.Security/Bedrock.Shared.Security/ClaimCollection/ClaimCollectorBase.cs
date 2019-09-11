using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using Bedrock.Shared.Configuration;
using Bedrock.Shared.Enumeration.StringHelper;

using Bedrock.Shared.Security.Model;
using Bedrock.Shared.Security.Interface;

using Bedrock.Shared.Utility;

namespace Bedrock.Shared.Security.ClaimCollection
{
    public abstract class ClaimCollectorBase : IClaimCollector, IDisposable
    {
        #region Fields
        private bool _disposed;
        #endregion

        #region Constructors
        public ClaimCollectorBase(BedrockConfiguration bedrockConfiguration)
        {
            BedrockConfiguration = bedrockConfiguration;
        }
        #endregion

        #region Protected Properties
        protected SecurityApplication ApplicationConfig
        {
            get { return BedrockConfiguration.Security.Application; }
        }

        protected BedrockConfiguration BedrockConfiguration { get; set; }
        #endregion

        #region IClaimCollector Methods
        public virtual IEnumerable<Claim> Collect(ICollectPass collectPass)
        {
            IEnumerable<BedrockClaimModel> returnValueModels;

            var resourceTypeProperty = StringHelper.Current.Lookup(StringSecurity.ResourceType);

            if (ApplicationConfig.ClaimCollection.IsCacheEnabled)
            {
                var expiry = new TimeSpan(0, ApplicationConfig.ClaimCollection.CacheExpiry, 0);
                var key = StringHelper.Current.Lookup(StringCacheKey.UserPermissions, collectPass.SubjectId, collectPass.Application.ToString().ToLower());

                returnValueModels = collectPass.Cache.Get(key, expiry, () => CollectClaims(collectPass));
            }
            else
                returnValueModels = CollectClaims(collectPass);

            return returnValueModels.Select(cm =>
            {
                var claim = new Claim
                (
                    cm.Type,
                    cm.Value,
                    cm.ValueType,
                    cm.Issuer,
                    cm.OriginalIssuer
                );

                claim.Properties.Add(resourceTypeProperty, cm.ResourceType);

                return claim;
            });
        }
        #endregion

        #region IDisposable Methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Protected Methods
        protected abstract IEnumerable<BedrockClaimModel> CollectClaims(ICollectPass collectPass);

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing) { }

            _disposed = true;
        }
        #endregion
    }
}

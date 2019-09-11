using System;

using Bedrock.Shared.Cache.Interface;
using Bedrock.Shared.Security.Interface;

namespace Bedrock.Shared.Security.ClaimCollection
{
    public class CollectPass : ICollectPass
    {
        #region Properties
        public string CollectPassName { get; set; }

        public IClaimCollector Collector { get; set; }

        public string Issuer { get; set; }

        public string Application { get; set; }

        public ICacheProvider Cache { get; set; }

        public string Username { get; set; }

        public string SubjectId { get; set; }
        #endregion

        #region Methods
        public void Validate()
        {
            if (Collector == null)
                throw new ArgumentNullException(nameof(Collector));

            if (string.IsNullOrEmpty(Issuer))
                throw new ArgumentException(nameof(Issuer));

            if (Application.Equals(default(string)))
                throw new ArgumentException(nameof(Application));

            if (Cache == null)
                throw new ArgumentNullException(nameof(Cache));

            if (string.IsNullOrEmpty(Username))
                throw new ArgumentException(nameof(Username));

            if (string.IsNullOrEmpty(SubjectId))
                throw new ArgumentException(nameof(SubjectId));
        }
        #endregion
    }
}

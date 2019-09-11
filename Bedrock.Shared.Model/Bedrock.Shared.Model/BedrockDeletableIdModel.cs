using System;
using Bedrock.Shared.Entity.Interface;

namespace Bedrock.Shared.Model
{
    public class BedrockDeletableIdModel<DM, M, K> : BedrockAuditIdModel<DM, M, K>, IBedrockIdEntity<K>, IBedrockDeletableEntity, IBedrockAuditEntity, IBedrockEntity
        where DM : class, new()
        where M : BedrockDeletableIdModel<DM, M, K>, new()
        where K : IComparable
    {
        #region IBedrockDeletableEntity Properties
        public bool IsDeleted { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }
        #endregion
    }
}

using System;
using Bedrock.Shared.Entity.Interface;

namespace Bedrock.Shared.Model
{
    public class BedrockAuditIdModel<DM, M, K> : MapperModelIdBase<DM, M, K>, IBedrockIdEntity<K>, IBedrockAuditEntity, IBedrockEntity
        where DM : class, new()
        where M : BedrockAuditIdModel<DM, M, K>, new()
        where K : IComparable
    {
        #region BedrockAuditEntity Properties
        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        #endregion
    }
}

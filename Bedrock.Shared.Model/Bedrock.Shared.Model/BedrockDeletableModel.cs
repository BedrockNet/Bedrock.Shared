using System;
using Bedrock.Shared.Entity.Interface;

namespace Bedrock.Shared.Model
{
    public class BedrockDeletableModel<DM, M> : BedrockAuditModel<DM, M>, IBedrockDeletableEntity, IBedrockAuditEntity, IBedrockEntity
        where DM : class, new()
        where M : BedrockDeletableModel<DM, M>, new()
    {
        #region IBedrockDeletableEntity Properties
        public bool IsDeleted { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }
        #endregion
    }
}

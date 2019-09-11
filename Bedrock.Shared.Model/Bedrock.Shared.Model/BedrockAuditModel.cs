using System;
using Bedrock.Shared.Entity.Interface;

namespace Bedrock.Shared.Model
{
    public class BedrockAuditModel<DM, M> : BedrockModel<DM, M>, IBedrockAuditEntity, IBedrockEntity
        where DM : class, new()
        where M : BedrockAuditModel<DM, M>, new()
    {
        #region IBedrockAuditEntity Properties
        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        #endregion
    }
}

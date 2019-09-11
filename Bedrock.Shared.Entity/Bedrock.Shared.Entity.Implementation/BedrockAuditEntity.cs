using System;

using Bedrock.Shared.Data.Validation.Interface;
using Bedrock.Shared.Entity.Interface;

namespace Bedrock.Shared.Entity.Implementation
{
    public class BedrockAuditEntity<TEntity> : BedrockEntity<TEntity>, IBedrockEntity, IBedrockAuditEntity, IValidatableEntity
        where TEntity : BedrockAuditEntity<TEntity>, IBedrockAuditEntity
    {
        #region IAuditableEntity Properties
        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        #endregion
    }
}

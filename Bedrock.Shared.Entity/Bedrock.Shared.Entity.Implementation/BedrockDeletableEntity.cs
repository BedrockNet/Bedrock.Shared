using System;
using System.ComponentModel.DataAnnotations.Schema;

using Bedrock.Shared.Data.Validation.Interface;
using Bedrock.Shared.Entity.Interface;

namespace Bedrock.Shared.Entity.Implementation
{
    public class BedrockDeletableEntity<TEntity> : BedrockAuditEntity<TEntity>, IBedrockEntity, IBedrockDeletableEntity, IBedrockAuditEntity, IValidatableEntity
        where TEntity : BedrockDeletableEntity<TEntity>, IBedrockDeletableEntity
    {
        #region IBedrockDeletableEntity Properties
        [NotMapped]
        public bool IsDeleted { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }
        #endregion
    }
}

using System;

using Bedrock.Shared.Data.Validation.Interface;
using Bedrock.Shared.Entity.Interface;

namespace Bedrock.Shared.Entity.Implementation
{
    public class BedrockAuditIdEntity<TEntity, TKey> : BedrockIdEntity<TEntity, TKey>, IBedrockEntity, IBedrockIdEntity<TKey>, IBedrockAuditEntity, IValidatableEntity
        where TEntity : BedrockAuditIdEntity<TEntity, TKey>, IBedrockAuditEntity
        where TKey : IComparable
    {
        #region IAuditableEntity Properties
        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        #endregion
    }
}

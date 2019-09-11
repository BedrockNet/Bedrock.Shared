using System;
using System.ComponentModel.DataAnnotations.Schema;

using Bedrock.Shared.Data.Validation.Interface;
using Bedrock.Shared.Entity.Interface;

namespace Bedrock.Shared.Entity.Implementation
{
    public class BedrockDeletableIdEntity<TEntity, TKey> : BedrockAuditIdEntity<TEntity, TKey>, IBedrockEntity, IBedrockIdEntity<TKey>, IBedrockDeletableEntity, IBedrockAuditEntity, IValidatableEntity
       where TEntity : BedrockDeletableIdEntity<TEntity, TKey>, IBedrockDeletableEntity
       where TKey : IComparable
    {
		#region IDeletableEntity Properties
		[NotMapped]
		public bool IsDeleted { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }
        #endregion
    }
}

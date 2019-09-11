using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Bedrock.Shared.Data.Validation.Interface;
using Bedrock.Shared.Domain.Interface;
using Bedrock.Shared.Entity.Interface;

namespace Bedrock.Shared.Entity.Implementation
{
    public class BedrockIdEntity<TEntity, TKey> : ValidatableIdEntity<TEntity, TKey>, IBedrockEntity, IBedrockIdEntity<TKey>, IValidatableEntity, IBedrockDomainEventEntity
        where TEntity : BedrockIdEntity<TEntity, TKey>, IBedrockEntity
        where TKey : IComparable
    {
        #region Constructors
        public BedrockIdEntity()
        {
            Events = new List<IDomainEvent>();
        }
        #endregion

        #region IBedrockDomainEventEntity Members
        [NotMapped]
        public IList<IDomainEvent> Events { get; private set; }

        public void ClearEvents()
        {
            Events.Clear();
        }
        #endregion
    }
}
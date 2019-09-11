using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Bedrock.Shared.Data.Validation.Interface;
using Bedrock.Shared.Domain.Interface;
using Bedrock.Shared.Entity.Interface;

namespace Bedrock.Shared.Entity.Implementation
{
    public class BedrockEntity<TEntity> : ValidatableEntity<TEntity>, IBedrockEntity, IValidatableEntity, IBedrockDomainEventEntity
        where TEntity : BedrockEntity<TEntity>, IBedrockEntity
    {
        #region Constructors
        public BedrockEntity()
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
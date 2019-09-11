using System.Collections.Generic;

namespace Bedrock.Shared.Domain.Interface
{
    public interface IBedrockDomainEventEntity
    {
        #region Properties
        IList<IDomainEvent> Events { get; }
        #endregion

        #region Methods
        void ClearEvents();
        #endregion
    }
}
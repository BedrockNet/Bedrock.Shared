using System.Collections.Generic;
using Bedrock.Shared.Session.Interface;

namespace Bedrock.Shared.Domain.Interface
{
	public interface IDomainEventDispatcher : ISessionAware
	{
		#region Methods
		void Dispatch(IBedrockDomainEventEntity entity, bool isWalkTree = true, bool isClearEvents = true);

		void Dispatch(IEnumerable<IBedrockDomainEventEntity> entities, bool isWalkTree = true, bool isClearEvents = true);

		void Dispatch(IDomainEvent domainEvent);
		#endregion
	}
}

using Bedrock.Shared.Domain.Interface;

namespace Bedrock.Shared.Domain.Implementation
{
	public abstract class DomainEventHandlerWrapperBase
	{
		#region Methods
		public abstract void Handle(IDomainEvent domainEvent);
		#endregion
	}
}

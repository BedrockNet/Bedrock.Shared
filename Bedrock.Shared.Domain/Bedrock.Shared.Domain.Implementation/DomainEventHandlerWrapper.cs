using Bedrock.Shared.Domain.Interface;

namespace Bedrock.Shared.Domain.Implementation
{
	public class DomainEventHandlerWrapper<T> : DomainEventHandlerWrapperBase where T : IDomainEvent
	{
		#region Fields
		private readonly IDomainEventHandler<T> _handler;
		#endregion

		#region Constructors
		public DomainEventHandlerWrapper(IDomainEventHandler<T> handler)
		{
			_handler = handler;
		}
		#endregion

		#region DomainEventHandlerBase Members
		public override void Handle(IDomainEvent domainEvent)
		{
			_handler.Handle((T)domainEvent);
		}
		#endregion
	}
}

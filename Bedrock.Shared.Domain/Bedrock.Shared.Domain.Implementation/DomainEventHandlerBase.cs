using Bedrock.Shared.Domain.Interface;
using Bedrock.Shared.Session.Implementation;
using Bedrock.Shared.Session.Interface;

namespace Bedrock.Shared.Domain.Implementation
{
	public abstract class DomainEventHandlerBase<T> : SessionAwareBase, IDomainEventHandler<T> where T : IDomainEvent
	{
		#region Constructors
		public DomainEventHandlerBase(params ISessionAware[] sessionAwareDependencies) : base(sessionAwareDependencies) { }
		#endregion

		#region IDomainEventHandler Methods
		public abstract void Handle(T domainEvent);
		#endregion
	}
}

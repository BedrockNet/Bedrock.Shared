namespace Bedrock.Shared.Domain.Interface
{
	public interface IDomainEventHandler<in T> where T : IDomainEvent
	{
		#region Methods
		void Handle(T domainEvent);
		#endregion
	}
}

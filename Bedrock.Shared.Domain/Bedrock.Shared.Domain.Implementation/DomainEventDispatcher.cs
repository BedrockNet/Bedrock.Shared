using System;
using System.Collections.Generic;
using System.Linq;

using Bedrock.Shared.Domain.Interface;
using Bedrock.Shared.Extension;
using Bedrock.Shared.Session.Implementation;
using Bedrock.Shared.Session.Interface;
using Bedrock.Shared.Utility;

using CommonServiceLocator;

namespace Bedrock.Shared.Domain.Implementation
{
	public class DomainEventDispatcher : SessionAwareBase, IDomainEventDispatcher
	{
		#region IDomainEventDispatcher Methods
		public void Dispatch(IBedrockDomainEventEntity entity, bool isWalkTree = true, bool isClearEvents = true)
		{
			if(isWalkTree)
				TreeWalker<IBedrockDomainEventEntity>.Current.Walk(entity, (ee) =>
				{
					ee.Events.Each(e => Dispatch(e));

					if (isClearEvents)
						ee.ClearEvents();
				});
			else
			{
				entity.Events.Each(e => Dispatch(e));

				if (isClearEvents)
					entity.ClearEvents();
			}
		}

		public void Dispatch(IEnumerable<IBedrockDomainEventEntity> entities, bool isWalkTree = true, bool isClearEvents = true)
		{
			if (isWalkTree)
				TreeWalker<IBedrockDomainEventEntity>.Current.Walk(entities, (ee) =>
				{
					ee.Events.Each(e => Dispatch(e));

					if(isClearEvents)
						ee.ClearEvents();
				});
			else
			{
				entities.Each(ee =>
				{
					ee.Events.Each(e => Dispatch(e));

					if (isClearEvents)
						ee.ClearEvents();
				});
			}
		}

		public void Dispatch(IDomainEvent domainEvent)
		{
			var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
			var wrapperType = typeof(DomainEventHandlerWrapper<>).MakeGenericType(domainEvent.GetType());
			var handlers = ServiceLocator.Current.GetAllInstances(handlerType);
			var wrappedHandlers = handlers
									.Cast<object>()
									.Select(handler => (DomainEventHandlerWrapperBase)Activator.CreateInstance(wrapperType, handler));

			handlers.Each(h =>
			{
				var sessionAware = h as ISessionAware;

				if (sessionAware != null)
					sessionAware.Enlist(Session);
			});

			wrappedHandlers.Each(h => h.Handle(domainEvent));
		}
		#endregion
	}
}

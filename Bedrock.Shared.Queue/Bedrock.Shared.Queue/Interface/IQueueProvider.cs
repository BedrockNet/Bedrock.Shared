using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Enumeration;

namespace Bedrock.Shared.Queue.Interface
{
    public interface IQueueProvider
    {
		#region Methods
		Task AddMessageAsync<TMessage, TMessageId>(IQueueMessage<TMessage, TMessageId> queueMessage, QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken));

		Task<IQueueMessage<TMessage, TMessageId>> GetMessageAsync<TMessage, TMessageId>(QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken));

		Task<IEnumerable<IQueueMessage<TMessage, TMessageId>>> GetMessagesAsync<TMessage, TMessageId>(QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken));

		Task<IQueueMessage<TMessage, TMessageId>> PeekMessageAsync<TMessage, TMessageId>(QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken));

		Task<IEnumerable<IQueueMessage<TMessage, TMessageId>>> PeekMessagesAsync<TMessage, TMessageId>(QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken));

		Task RemoveMessageAsync<TMessage, TMessageId>(TMessageId id, QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken));

		Task RemoveMessagesAsync<TMessage, TMessageId>(TMessageId[] ids, QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken));

		Task<int> GetMessageCountAsync<TMessage, TMessageId>(QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken));

		Task CleanQueueAsync<TMessage, TMessageId>(QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken));
        #endregion
    }
}

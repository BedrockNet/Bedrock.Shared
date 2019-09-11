using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Enumeration;

namespace Bedrock.Shared.Queue.Interface
{
    public interface IQueue
    {
        QueueType QueueType { get; }
    }

    public interface IQueue<TMessage, TMessageId> : IQueue
    {
        Task AddMessageAsync(IQueueMessage<TMessage, TMessageId> queueMessage, QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));

		Task<IQueueMessage<TMessage, TMessageId>> GetMessageAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));

		Task<IEnumerable<IQueueMessage<TMessage, TMessageId>>> GetMessagesAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));

		Task<IQueueMessage<TMessage, TMessageId>> PeekMessageAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));

		Task<IEnumerable<IQueueMessage<TMessage, TMessageId>>> PeekMessagesAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));

		Task RemoveMessageAsync(TMessageId id, QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));

		Task RemoveMessagesAsync(TMessageId[] ids, QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));

		Task<int> GetMessageCountAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));

		Task CleanQueueAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Configuration;
using Bedrock.Shared.Enumeration;
using Bedrock.Shared.Queue.Interface;

namespace Bedrock.Shared.Queue
{
    public class QueueProvider : IQueueProvider
    {
        #region Constructors
        public QueueProvider(IQueue[] queues, BedrockConfiguration bedrockConfiguration) : this(queues, bedrockConfiguration.Queue.QueueType) { }

        public QueueProvider(IQueue[] queues, QueueType queueType)
        {
            Initialize(queues, queueType);
        }
        #endregion

        #region Protected properties
        protected IEnumerable<IQueue> Queues { get; set; }

        protected QueueType QueueType { get; private set; }
        #endregion

        #region IQueueProvider Methods
        public Task AddMessageAsync<TMessage, TMessageId>(IQueueMessage<TMessage, TMessageId> queueMessage, QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetQueue<TMessage, TMessageId>(queueType).AddMessageAsync(queueMessage, queueSettings, cancellationToken);
        }

        public Task<IQueueMessage<TMessage, TMessageId>> GetMessageAsync<TMessage, TMessageId>(QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetQueue<TMessage, TMessageId>(queueType).GetMessageAsync(queueSettings, cancellationToken);
        }

        public Task<IEnumerable<IQueueMessage<TMessage, TMessageId>>> GetMessagesAsync<TMessage, TMessageId>(QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var returnValue = new List<IQueueMessage<TMessage, TMessageId>>();
            return GetQueue<TMessage, TMessageId>(queueType).GetMessagesAsync(queueSettings, cancellationToken);
        }

        public Task<IQueueMessage<TMessage, TMessageId>> PeekMessageAsync<TMessage, TMessageId>(QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetQueue<TMessage, TMessageId>(queueType).PeekMessageAsync(queueSettings, cancellationToken);
        }

        public Task<IEnumerable<IQueueMessage<TMessage, TMessageId>>> PeekMessagesAsync<TMessage, TMessageId>(QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetQueue<TMessage, TMessageId>(queueType).PeekMessagesAsync(queueSettings, cancellationToken);
        }

        public Task RemoveMessageAsync<TMessage, TMessageId>(TMessageId id, QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetQueue<TMessage, TMessageId>(queueType).RemoveMessageAsync(id, queueSettings, cancellationToken);
        }

        public Task RemoveMessagesAsync<TMessage, TMessageId>(TMessageId[] ids, QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetQueue<TMessage, TMessageId>(queueType).RemoveMessagesAsync(ids, queueSettings, cancellationToken);
        }

        public Task<int> GetMessageCountAsync<TMessage, TMessageId>(QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetQueue<TMessage, TMessageId>(queueType).GetMessageCountAsync(queueSettings, cancellationToken);
        }

        public Task CleanQueueAsync<TMessage, TMessageId>(QueueSettings queueSettings, QueueType? queueType = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetQueue<TMessage, TMessageId>(queueType).CleanQueueAsync(queueSettings, cancellationToken);
        }
        #endregion

        #region Private Methods
        private void Initialize(IQueue[] queues, QueueType queueType)
        {
            Queues = queues;
            QueueType = queueType;
        }

        private IQueue<TMessage, TMessageId> GetQueue<TMessage, TMessageId>(QueueType? queueType = null)
        {
            queueType = queueType ?? QueueType;
            return (IQueue<TMessage, TMessageId>)Queues.First(q => q.QueueType == queueType);
        }
        #endregion
    }
}

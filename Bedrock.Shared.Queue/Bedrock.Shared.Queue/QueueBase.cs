using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Configuration;
using Bedrock.Shared.Enumeration;
using Bedrock.Shared.Queue.Interface;

namespace Bedrock.Shared.Queue
{
    public abstract class QueueBase<TMessage, TMessageId> : IQueue<TMessage, TMessageId>
    {
		#region Constructors
		public QueueBase(BedrockConfiguration bedrockConfiguration)
		{
			BedrockConfiguration = bedrockConfiguration;
		}
		#endregion

		#region Public Properties
		public abstract QueueType QueueType { get; }
		#endregion

		#region Protected Properties
		protected BedrockConfiguration BedrockConfiguration { get; set; }
		#endregion

		#region IQueue Methods
		public abstract Task AddMessageAsync(IQueueMessage<TMessage, TMessageId> queueMessage, QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));

        public abstract Task<IQueueMessage<TMessage, TMessageId>> GetMessageAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));

        public abstract Task<IEnumerable<IQueueMessage<TMessage, TMessageId>>> GetMessagesAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));

        public abstract Task<IQueueMessage<TMessage, TMessageId>> PeekMessageAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));

        public abstract Task<IEnumerable<IQueueMessage<TMessage, TMessageId>>> PeekMessagesAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));

        public abstract Task RemoveMessageAsync(TMessageId id, QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));

        public abstract Task RemoveMessagesAsync(TMessageId[] ids, QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));

        public abstract Task<int> GetMessageCountAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));

        public abstract Task CleanQueueAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken));
        #endregion
    }
}

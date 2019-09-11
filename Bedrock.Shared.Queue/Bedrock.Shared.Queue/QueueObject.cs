using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Enumeration;
using Bedrock.Shared.Extension;
using Bedrock.Shared.Queue.Interface;
using Bedrock.Shared.Serialization.Binary;

namespace Bedrock.Shared.Queue
{
    public class QueueObject : QueueBase<object, int>, IQueueObject
    {
        #region Instance Variables
        private static readonly object _syncRoot = new object();
        private static volatile QueueObject _instance;

        private Dictionary<string, QueueObject> _queues;
        private Dictionary<int, IQueueMessage<object, int>> _queue;
        #endregion

        #region Constructors
        private QueueObject() : base(null) { }
        #endregion

        #region IQueue Properties
        public override QueueType QueueType => QueueType.Object;
        #endregion

        #region Properties
        protected Dictionary<string, QueueObject> Queues
        {
            get
            {
                _queues = _queues ?? new Dictionary<string, QueueObject>();
                return _queues;
            }
        }

        protected Dictionary<int, IQueueMessage<object, int>> Queue
        {
            get
            {
                _queue = _queue ?? new Dictionary<int, IQueueMessage<object, int>>();
                return _queue;
            }
        }

        private int IdInternal { get; set; }
        #endregion

        #region Public Static Methods
        public static QueueObject GetQueue(string queueName)
        {
            QueueObject returnValue;

            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new QueueObject();
                }
            }

            returnValue = _instance.GetQueueInternal(queueName);

            if (returnValue == null)
                returnValue = _instance.CreateQueue(queueName);

            return returnValue;
        }
        #endregion

        #region IQueue Methods
        public override Task AddMessageAsync(IQueueMessage<object, int> message, QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken))
        {
			message.Id = IdInternal;

			Queue.Add(IdInternal, SerializerBinary.CloneStatic(message));
			++IdInternal;

			return Task.Delay(0, cancellationToken);
        }

        public override Task<IQueueMessage<object, int>> GetMessageAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken))
        {
            var returnValue = Task.FromResult(default(IQueueMessage<object, int>));

            if (Queue.Any())
            {
                var pair = Queue.First();

                returnValue = Task.FromResult(SerializerBinary.CloneStatic(pair.Value));
                Queue.Remove(pair.Key);
            }

            return returnValue;
        }

        public override Task<IEnumerable<IQueueMessage<object, int>>> GetMessagesAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken))
        {
			var values = Queue
						.Values
						.Select(m => SerializerBinary.CloneStatic(m))
						.ToList()
						.AsEnumerable();

            CleanQueueAsync(queueSettings);

            return Task.FromResult(values);
        }

        public override Task<IQueueMessage<object, int>> PeekMessageAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken))
        {
            var returnValue = Task.FromResult(default(IQueueMessage<object, int>));

            if (Queue.Any())
            {
                var pair = Queue.First();
                returnValue = Task.FromResult(SerializerBinary.CloneStatic(pair.Value));
            }

            return returnValue;
        }

        public override Task<IEnumerable<IQueueMessage<object, int>>> PeekMessagesAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken))
        {
            var values = Queue
						.Values
						.Select(m => SerializerBinary.CloneStatic(m))
						.ToList()
						.AsEnumerable();

			return Task.FromResult(values);
        }

        public override Task RemoveMessageAsync(int id, QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken))
        {
            Queue.Remove(id);
			return Task.Delay(0, cancellationToken);
        }

        public override Task RemoveMessagesAsync(int[] ids, QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken))
        {
            ids.Each(i => Queue.Remove(i));
			return Task.Delay(0, cancellationToken);
		}

        public override Task<int> GetMessageCountAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(Queue.Count());
        }

        public override Task CleanQueueAsync(QueueSettings queueSettings, CancellationToken cancellationToken = default(CancellationToken))
        {
            Queue.Clear();
			return Task.Delay(0, cancellationToken);
		}
        #endregion

        #region IDisposable Methods
        public void Dispose()
        {
            _instance = null;
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Private Methods
        private QueueObject GetQueueInternal(string queueName)
        {
            var returnValue = default(QueueObject);

            if (Queues.ContainsKey(queueName))
                returnValue = Queues[queueName];

            return returnValue;
        }

        private QueueObject CreateQueue(string queueName)
        {
            var queue = new QueueObject();
            Queues.Add(queueName, queue);
            return queue;
        }
        #endregion
    }
}

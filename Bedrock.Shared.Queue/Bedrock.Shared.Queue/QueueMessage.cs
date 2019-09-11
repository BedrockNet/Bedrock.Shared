using System;
using Bedrock.Shared.Queue.Interface;

namespace Bedrock.Shared.Queue
{
    [Serializable]
    public class QueueMessage<TMessage, TMessageId> : IQueueMessage<TMessage, TMessageId>
    {
        #region IQueueMessage Properties
        public TMessageId Id { get; set; }

        public TMessage Body { get; set; }

        public DateTime ReceiveTimeStamp { get; set; }
        #endregion

        #region Public Static Methods
        public static IQueueMessage<TMessage, TMessageId> Create()
        {
            return new QueueMessage<TMessage, TMessageId>();
        }

        public static IQueueMessage<TMessage, TMessageId> Create(TMessage body)
        {
            return new QueueMessage<TMessage, TMessageId>()
            {
                Body = body
            };
        }

        public static IQueueMessage<TMessage, TMessageId> Create(TMessageId id, TMessage body, DateTime receiveTimeStamp)
        {
            return new QueueMessage<TMessage, TMessageId>
            {
                Id = id,
                Body = body,
                ReceiveTimeStamp = receiveTimeStamp
            };
        }
        #endregion
    }
}

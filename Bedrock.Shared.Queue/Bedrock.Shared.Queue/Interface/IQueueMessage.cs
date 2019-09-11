using System;

namespace Bedrock.Shared.Queue.Interface
{
    public interface IQueueMessage<TMessage, TMessageId>
    {
        TMessageId Id { get; set; }

        TMessage Body { get; set; }

        DateTime ReceiveTimeStamp { get; set; }
    }
}

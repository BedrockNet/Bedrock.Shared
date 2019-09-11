using System;

namespace Bedrock.Shared.Queue.Interface
{
    public interface IQueueObject : IQueue<object, int>, IDisposable { }
}

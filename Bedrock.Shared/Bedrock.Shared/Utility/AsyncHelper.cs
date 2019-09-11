using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bedrock.Shared.Utility
{
    public static class AsyncHelper
    {
        #region Fields
        private static readonly TaskFactory _taskFactory;
        #endregion

        #region Constructors
        static AsyncHelper()
        {
            _taskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);
        }
        #endregion

        #region Public Methods
        public static void RunSync(Func<Task> func)
        {
            _taskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return _taskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }
        #endregion
    }
}

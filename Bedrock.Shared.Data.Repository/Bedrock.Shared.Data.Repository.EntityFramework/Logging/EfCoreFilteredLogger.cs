using System;
using System.Diagnostics;

using Bedrock.Shared.Configuration;
using Bedrock.Shared.Utility;

using Microsoft.Extensions.Logging;
using SharedInterface = Bedrock.Shared.Log.Interface;

namespace Bedrock.Shared.Data.Repository.EntityFramework.Logging
{
    public class EfCoreFilteredLogger : ILogger
    {
        #region Fields
        private readonly SharedInterface.ILogger _internalLogger;
        #endregion

        #region Constructors
        public EfCoreFilteredLogger(BedrockConfiguration bedrockConfiguration, SharedInterface.ILogger internalLogger = null)
        {
            BedrockConfiguration = bedrockConfiguration;
            _internalLogger = internalLogger;
        }
        #endregion

        #region Properties
        protected BedrockConfiguration BedrockConfiguration { get; set; }
        #endregion

        #region ILogger Members
        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);

            if (BedrockConfiguration.Log.Orm.IsLog)
                Log(logLevel, message, exception);

            if (BedrockConfiguration.Log.Orm.IsConsole)
                Console.WriteLine(message);

            if (BedrockConfiguration.Log.Orm.IsDebug)
                Debug.WriteLine(message);
        }
        #endregion

        #region Private Methods
        private void Log(LogLevel logLevel, string message, Exception exception)
        {
            if (_internalLogger == null)
                return;

            switch (logLevel)
            {
                case LogLevel.Critical:
                    {
                        _internalLogger.Fatal(exception, message);
                        break;
                    }
                case LogLevel.Debug:
                    {
                        _internalLogger.Debug(exception, message);
                        break;
                    }
                case LogLevel.Error:
                    {
                        _internalLogger.Error(exception, message);
                        break;
                    }
                case LogLevel.Information:
                    {
                        _internalLogger.Info(exception, message);
                        break;
                    }
                case LogLevel.Trace:
                    {
                        _internalLogger.Trace(exception, message);
                        break;
                    }
                case LogLevel.Warning:
                    {
                        _internalLogger.Warn(exception, message);
                        break;
                    }
            }
        }
        #endregion
    }
}

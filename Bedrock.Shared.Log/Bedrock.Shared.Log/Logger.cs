using System;

using Bedrock.Shared.Log.Extension;
using Bedrock.Shared.Utility;

using Microsoft.Extensions.Logging;
using SharedInterface = Bedrock.Shared.Log.Interface;

namespace Bedrock.Shared.Log
{
	public class Logger : ILogger
	{
		#region Constructors
		private string _categoryName;
		private Func<string, LogLevel, bool> _filter;
		private SharedInterface.ILogger _internalLogger;

		public Logger(string categoryName, Func<string, LogLevel, bool> filter, SharedInterface.ILogger internalLogger)
		{
			_categoryName = categoryName;
			_filter = filter;
			_internalLogger = internalLogger;
		}
		#endregion

		#region ILogger Members
		public IDisposable BeginScope<TState>(TState state)
		{
			return new NoopDisposable();
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return ((_filter == null || _filter(_categoryName, logLevel)) && _internalLogger.IsEnabled(logLevel.ToLogLevel()));
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
				return;

			if (formatter == null)
				throw new ArgumentNullException(nameof(formatter));

			var message = formatter(state, exception);

			if (string.IsNullOrEmpty(message))
				return;

			Log(logLevel, message, exception);
		}
		#endregion

		#region Private Methods
		private void Log(LogLevel logLevel, string message, Exception exception)
		{
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

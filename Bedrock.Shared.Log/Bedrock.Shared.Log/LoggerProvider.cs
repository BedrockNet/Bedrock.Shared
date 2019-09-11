using System;
using Microsoft.Extensions.Logging;
using SharedInterface = Bedrock.Shared.Log.Interface;

namespace Bedrock.Shared.Log
{
	public class LoggerProvider : ILoggerProvider
	{
		#region Fields
		private readonly Func<string, LogLevel, bool> _filter;
		private readonly SharedInterface.ILogger _internalLogger;
		#endregion

		#region Constructors
		public LoggerProvider(Func<string, LogLevel, bool> filter, SharedInterface.ILogger internalLogger)
		{
			_filter = filter;
			_internalLogger = internalLogger;
		}
		#endregion

		#region ILoggerProvider Members
		public ILogger CreateLogger(string categoryName)
		{
			return new Logger(categoryName, _filter, _internalLogger);
		}
		#endregion

		#region IDisposable Members
		public void Dispose() { }
		#endregion
	}
}

using System;
using Bedrock.Shared.Utility;
using Microsoft.Extensions.Logging;

namespace Bedrock.Shared.Log
{
	public class NullLogger : ILogger
	{
		#region ILogger Members
		public IDisposable BeginScope<TState>(TState state)
		{
			return new NoopDisposable();
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return false;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) { }
		#endregion
	}
}

using System;
using Microsoft.Extensions.Logging;
using SharedInterface = Bedrock.Shared.Log.Interface;

namespace Bedrock.Shared.Log.Extension
{
	public static class LoggerFactoryExtension
	{
		#region Public Methods
		public static ILoggerFactory AddLogger(this ILoggerFactory factory, SharedInterface.ILogger logger, LogLevel minLevel)
		{
			return AddLogger(factory, logger, (_, logLevel) => logLevel >= minLevel);
		}

		public static ILoggerFactory AddLogger(this ILoggerFactory factory, SharedInterface.ILogger logger, Func<string, LogLevel, bool> filter = null)
		{
			factory.AddProvider(new LoggerProvider(filter, logger));
			return factory;
		}
		#endregion
	}
}

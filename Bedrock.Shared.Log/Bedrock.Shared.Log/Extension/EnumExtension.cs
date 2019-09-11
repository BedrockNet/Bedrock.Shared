using Microsoft.Extensions.Logging;
using SharedLogEnumeration = Bedrock.Shared.Log.Enumeration;

namespace Bedrock.Shared.Log.Extension
{
	public static class EnumExtension
	{
		#region Public Methods
		public static SharedLogEnumeration.LogLevel ToLogLevel(this LogLevel logLevel)
		{
			switch (logLevel)
			{
				case LogLevel.Critical:
					return SharedLogEnumeration.LogLevel.Critical;
				case LogLevel.Debug:
					return SharedLogEnumeration.LogLevel.Debug;
				case LogLevel.Error:
					return SharedLogEnumeration.LogLevel.Error;
				case LogLevel.Information:
					return SharedLogEnumeration.LogLevel.Information;
				case LogLevel.Trace:
					return SharedLogEnumeration.LogLevel.Trace;
				case LogLevel.Warning:
					return SharedLogEnumeration.LogLevel.Warning;
				default:
					return SharedLogEnumeration.LogLevel.None;
			}
		}
		#endregion
	}
}

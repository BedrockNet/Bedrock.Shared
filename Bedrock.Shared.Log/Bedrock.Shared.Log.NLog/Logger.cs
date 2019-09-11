using System;
using System.Diagnostics;
using System.Linq;

using Bedrock.Shared.Configuration;
using Bedrock.Shared.Extension;

using NLog;
using NLog.Config;

using LogEnumeration = Bedrock.Shared.Log.Enumeration;

namespace Bedrock.Shared.Log.NLog
{
    public class Logger : LoggerBase
    {
        #region Fields
        private readonly static Type _declaringType = typeof(Logger);
        private ILogger _defaultInstance;
        #endregion

        #region Constructors
        public Logger(BedrockConfiguration bedrockConfiguration) : base(bedrockConfiguration)
        {
			LogManager.Configuration = new XmlLoggingConfiguration($"nlog.{bedrockConfiguration.Application.Environment.ToString()}.config");
			_defaultInstance = LogManager.GetLogger(_declaringType.Name);
        }
        #endregion

        #region ILog Properties
        public override bool IsDebugEnabled => _defaultInstance.IsDebugEnabled;

        public override bool IsErrorEnabled => _defaultInstance.IsErrorEnabled;

        public override bool IsFatalEnabled => _defaultInstance.IsFatalEnabled;

        public override bool IsInfoEnabled => _defaultInstance.IsInfoEnabled;

        public override bool IsTraceEnabled => _defaultInstance.IsTraceEnabled;

        public override bool IsWarnEnabled => _defaultInstance.IsWarnEnabled;
        #endregion

        #region ILog Methods
        public override bool IsEnabled(LogEnumeration.LogLevel logLevel)
        {
            var returnValue = false;

            switch (logLevel)
            {
                case LogEnumeration.LogLevel.Debug:
                    return IsDebugEnabled;
                case LogEnumeration.LogLevel.Error:
                    return IsErrorEnabled;
                case LogEnumeration.LogLevel.Fatal:
                    return IsFatalEnabled;
                case LogEnumeration.LogLevel.Information:
                    return IsInfoEnabled;
                case LogEnumeration.LogLevel.Trace:
                    return IsTraceEnabled;
                case LogEnumeration.LogLevel.Warning:
                    return IsWarnEnabled;
            }

            return returnValue;
        }
        #endregion

        #region ILog Methods (Trace)
        public override void Trace(Exception exception, string format, params string[] args)
        {
            if (!IsTraceEnabled)
                return;

            var logEvent = GetLogEvent(_declaringType.Name, LogLevel.Trace, exception, format, args);
            _defaultInstance.Log(typeof(Logger), logEvent);
        }
        #endregion

        #region ILog Methods (Debug)
        public override void Debug(Exception exception, string format, params string[] args)
        {
            if (!IsDebugEnabled)
                return;

            var logEvent = GetLogEvent(_declaringType.Name, LogLevel.Debug, exception, format, args);
            _defaultInstance.Log(typeof(Logger), logEvent);
        }
        #endregion

        #region ILog Methods (Info)
        public override void Info(Exception exception, string format, params string[] args)
        {
            if (!IsInfoEnabled)
                return;

            var logEvent = GetLogEvent(_declaringType.Name, LogLevel.Info, exception, format, args);
            _defaultInstance.Log(typeof(Logger), logEvent);
        }
        #endregion

        #region ILog Methods (Warn)
        public override void Warn(Exception exception, string format, params string[] args)
        {
            if (!IsWarnEnabled)
                return;

            var logEvent = GetLogEvent(_declaringType.Name, LogLevel.Warn, exception, format, args);
            _defaultInstance.Log(typeof(Logger), logEvent);
        }
        #endregion

        #region ILog Methods (Error)
        public override void Error(Exception exception, string format, params string[] args)
        {
            if (!IsErrorEnabled)
                return;

            var logEvent = GetLogEvent(_declaringType.Name, LogLevel.Error, exception, format, args);
            _defaultInstance.Log(typeof(Logger), logEvent);
        }
        #endregion

        #region ILog Methods (Fatal)
        public override void Fatal(Exception exception, string format, params string[] args)
        {
            if (!IsFatalEnabled)
                return;

            var logEvent = GetLogEvent(_declaringType.Name, LogLevel.Fatal, exception, format, args);
            _defaultInstance.Log(typeof(Logger), logEvent);
        }
        #endregion

        #region Private Methods
        private LogEventInfo GetLogEvent(string loggerName, LogLevel level, Exception exception, string format, object[] args)
        {
            var message = args.Length == 0 ? format : string.Format(format, args);
            var returnValue = new LogEventInfo(level, loggerName, null, message, null, exception);

            var application = string.Empty;
            var exceptionSource = string.Empty;
            var exceptionClass = string.Empty;
            var exceptionMethod = string.Empty;
            var exceptionError = string.Empty;
            var exceptionStackTrace = string.Empty;
            var exceptionInnerMessage = string.Empty;
            var exceptionFileName = string.Empty;

            application = BedrockConfiguration.Application.Name;

            if (exception != null)
            {
                exceptionSource = exception.Source;

                var frame = new StackTrace(exception, true).GetFrames()?.FirstOrDefault();

                if (frame != null)
                {
                    var methodInfo = frame.GetMethod();

                    exceptionClass = methodInfo?.DeclaringType?.FullName;
                    exceptionMethod = methodInfo.Name;
                    exceptionFileName = frame.GetFileName();
                }

                exceptionError = exception.Message;
                exceptionStackTrace = exception.StackTrace;
                exceptionInnerMessage = exception.GetInnermostExceptionMessage();
            }

            returnValue.Properties[nameof(application).ToUpperFirstLetter()] = application;
            returnValue.Properties[nameof(exceptionSource).ToUpperFirstLetter()] = exceptionSource;
            returnValue.Properties[nameof(exceptionClass).ToUpperFirstLetter()] = exceptionClass;
            returnValue.Properties[nameof(exceptionMethod).ToUpperFirstLetter()] = exceptionMethod;
            returnValue.Properties[nameof(exceptionError).ToUpperFirstLetter()] = exceptionError;
            returnValue.Properties[nameof(exceptionStackTrace).ToUpperFirstLetter()] = exceptionStackTrace;
            returnValue.Properties[nameof(exceptionInnerMessage).ToUpperFirstLetter()] = exceptionInnerMessage;
            returnValue.Properties[nameof(exceptionFileName).ToUpperFirstLetter()] = exceptionFileName;

            return returnValue;
        }
        #endregion
    }
}

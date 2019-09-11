using System;

using Bedrock.Shared.Configuration;
using Bedrock.Shared.Log.Enumeration;
using Bedrock.Shared.Log.Interface;

namespace Bedrock.Shared.Log
{
    public abstract class LoggerBase : ILogger
    {
		#region Constructors
		public LoggerBase(BedrockConfiguration bedrockConfiguration)
		{
			BedrockConfiguration = bedrockConfiguration;
		}
		#endregion

		#region Protected Properties
		protected BedrockConfiguration BedrockConfiguration { get; set; }
		#endregion

		#region ILog Properties
		public abstract bool IsDebugEnabled { get; }

        public abstract bool IsErrorEnabled { get; }

        public abstract bool IsFatalEnabled { get; }

        public abstract bool IsInfoEnabled { get; }

        public abstract bool IsTraceEnabled { get; }

        public abstract bool IsWarnEnabled { get; }
        #endregion

        #region ILog Methods
        public abstract bool IsEnabled(LogLevel logLevel);
        #endregion

        #region ILog Methods (Trace)
        public virtual void Trace(Exception exception)
        {
            Trace(exception, string.Empty);
        }

        public virtual void Trace(string format, params string[] args)
        {
            Trace(null, format, args);
        }

        public abstract void Trace(Exception exception, string format, params string[] args);
        #endregion

        #region ILog Methods (Debug)
        public virtual void Debug(Exception exception)
        {
            Debug(exception, string.Empty);
        }

        public virtual void Debug(string format, params string[] args)
        {
            Debug(null, format, args);
        }

        public abstract void Debug(Exception exception, string format, params string[] args);
        #endregion

        #region ILog Methods (Info)
        public virtual void Info(Exception exception)
        {
            Info(exception, string.Empty);
        }

        public virtual void Info(string format, params string[] args)
        {
            Info(null, format, args);
        }

        public abstract void Info(Exception exception, string format, params string[] args);
        #endregion

        #region ILog Methods (Warn)
        public virtual void Warn(Exception exception)
        {
            Warn(exception, string.Empty);
        }

        public virtual void Warn(string format, params string[] args)
        {
            Warn(null, format, args);
        }

        public abstract void Warn(Exception exception, string format, params string[] args);
        #endregion

        #region ILog Methods (Error)
        public virtual void Error(Exception exception)
        {
            Error(exception, string.Empty);
        }

        public virtual void Error(string format, params string[] args)
        {
            Error(null, format, args);
        }

        public abstract void Error(Exception exception, string format, params string[] args);
        #endregion

        #region ILog Methods (Fatal)
        public virtual void Fatal(Exception exception)
        {
            Fatal(exception, string.Empty);
        }

        public virtual void Fatal(string format, params string[] args)
        {
            Fatal(null, format, args);
        }

        public abstract void Fatal(Exception exception, string format, params string[] args);
        #endregion
    }
}

using System;
using Bedrock.Shared.Log.Enumeration;

namespace Bedrock.Shared.Log.Interface
{
    public interface ILogger
    {
        #region Properties
        bool IsDebugEnabled { get; }

        bool IsErrorEnabled { get; }

        bool IsFatalEnabled { get; }

        bool IsInfoEnabled { get; }

        bool IsTraceEnabled { get; }

        bool IsWarnEnabled { get; }
        #endregion

        #region Methods
        bool IsEnabled(LogLevel logLevel);
        #endregion

        #region Methods (Trace)
        void Trace(Exception exception);

        void Trace(string format, params string[] args);

        void Trace(Exception exception, string format, params string[] args);
        #endregion

        #region Methods (Debug)
        void Debug(Exception exception);

        void Debug(string format, params string[] args);

        void Debug(Exception exception, string format, params string[] args);
        #endregion

        #region Methods (Info)
        void Info(Exception exception);

        void Info(string format, params string[] args);

        void Info(Exception exception, string format, params string[] args);
        #endregion

        #region Methods (Warn)
        void Warn(Exception exception);

        void Warn(string format, params string[] args);

        void Warn(Exception exception, string format, params string[] args);
        #endregion

        #region Methods (Error)
        void Error(Exception exception);

        void Error(string format, params string[] args);

        void Error(Exception exception, string format, params string[] args);
        #endregion

        #region Methods (Fatal)
        void Fatal(Exception exception);

        void Fatal(string format, params string[] args);

        void Fatal(Exception exception, string format, params string[] args);
        #endregion
    }
}

using System;

namespace Bedrock.Shared.Extension
{
    public static class ExceptionExtension
    {
        #region Public Methods
        public static string GetInnermostExceptionMessage(this Exception exception)
        {
            var returnValue = string.Empty;
            var isFoundInner = false;

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                isFoundInner = true;
            }

            if (isFoundInner)
                returnValue = exception.Message;

            return returnValue;
        }

        public static Exception GetInnermostException(this Exception exception)
        {
            var returnValue = exception;
            var isFoundInner = false;

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                isFoundInner = true;
            }

            if (isFoundInner)
                returnValue = exception;

            return returnValue;
        }
        #endregion
    }
}

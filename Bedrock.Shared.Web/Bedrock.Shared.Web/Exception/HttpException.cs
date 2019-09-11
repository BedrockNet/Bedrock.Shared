using System.Net;

namespace Bedrock.Shared.Web.Exception
{
    public class HttpException : System.Exception
    {
        #region Public Properties
        public int StatusCode { get; private set; }

        public object Content { get; private set; }
        #endregion

        #region Constructors
        public HttpException(int httpStatusCode)
        {
            StatusCode = httpStatusCode;
        }

        public HttpException(HttpStatusCode httpStatusCode)
        {
            StatusCode = (int)httpStatusCode;
        }

        public HttpException(int httpStatusCode, string message) : base(message)
        {
            StatusCode = httpStatusCode;
        }

        public HttpException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            StatusCode = (int)httpStatusCode;
        }

        public HttpException(int httpStatusCode, string message, object content) : base(message)
        {
            StatusCode = httpStatusCode;
            Content = content;
        }

        public HttpException(HttpStatusCode httpStatusCode, string message, object content) : base(message)
        {
            StatusCode = (int)httpStatusCode;
            Content = content;
        }

        public HttpException(int httpStatusCode, string message, System.Exception inner) : base(message, inner)
        {
            StatusCode = httpStatusCode;
        }

        public HttpException(HttpStatusCode httpStatusCode, string message, System.Exception inner) : base(message, inner)
        {
            StatusCode = (int)httpStatusCode;
        }

        public HttpException(int httpStatusCode, string message, object content, System.Exception inner) : base(message, inner)
        {
            StatusCode = httpStatusCode;
            Content = content;
        }

        public HttpException(HttpStatusCode httpStatusCode, string message, object content, System.Exception inner) : base(message, inner)
        {
            StatusCode = (int)httpStatusCode;
            Content = content;
        }
        #endregion
    }
}

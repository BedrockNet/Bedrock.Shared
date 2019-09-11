using System.Collections.Generic;

namespace Bedrock.Shared.Web.Client.Response
{
    public class WebApiResponse
    {
        #region Properties
        public WebApiResponseMessage ResponseMessage { get; set; }

        public IEnumerable<WebApiResponseError> Errors { get; set; }
        #endregion

        #region Public Methods
        public static WebApiResponse Create(WebApiResponseMessage responseMessage, IEnumerable<WebApiResponseError> errors)
        {
            return new WebApiResponse
            {
                ResponseMessage = responseMessage,
                Errors = errors
            };
        }
        #endregion
    }

    public class WebApiResponse<T> : WebApiResponse
    {
        #region Properties
        public T ResponseValue { get; set; }
        #endregion

        #region Public Methods
        public static WebApiResponse<T> Create(T responseValue, WebApiResponseMessage responseMessage, IEnumerable<WebApiResponseError> errors)
        {
            return new WebApiResponse<T>
            {
                ResponseValue = responseValue,
                ResponseMessage = responseMessage,
                Errors = errors
            };
        }
        #endregion
    }
}

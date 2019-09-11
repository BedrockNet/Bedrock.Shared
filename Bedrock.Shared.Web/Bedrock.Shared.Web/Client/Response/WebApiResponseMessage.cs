using System;

namespace Bedrock.Shared.Web.Client.Response
{
    public class WebApiResponseMessage
    {
        #region Properties
        public bool IsSuccessStatusCode { get; set; }

        public int StatusCode { get; set; }

        public string ReasonPhrase { get; set; }

        public string ContentType { get; set; }

        public Uri LocationHeader { get; set; }
        #endregion

        #region Public Methods
        public static WebApiResponseMessage Create(bool isSuccessStatusCode, object statusCode, string reasonPhrase = null, string contentType = null, Uri locationHeader = null)
        {
            return new WebApiResponseMessage
			{
                IsSuccessStatusCode = isSuccessStatusCode,
                StatusCode = (int)statusCode,
                ReasonPhrase = reasonPhrase,
                ContentType = contentType,
                LocationHeader = locationHeader
            };
        }
        #endregion
    }
}

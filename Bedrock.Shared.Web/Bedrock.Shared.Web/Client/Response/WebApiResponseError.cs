using System.Collections.Generic;
using Bedrock.Shared.Extension;

namespace Bedrock.Shared.Web.Client.Response
{
    public class WebApiResponseError
    {
        #region Public Properties
        public string Key { get; set; }

        public string Value { get; set; }
        #endregion

        #region Public Methods
        public static WebApiResponseError CreateInstance(string key, string value)
        {
            return new WebApiResponseError
            {
                Key = key,
                Value = value
            };
        }

        public static List<WebApiResponseError> CreateListFromStringValues(params string[] values)
        {
            var returnValue = new List<WebApiResponseError>();
            values.Each(v => returnValue.Add(new WebApiResponseError { Value = v }));
            return returnValue;
        }
        #endregion
    }
}

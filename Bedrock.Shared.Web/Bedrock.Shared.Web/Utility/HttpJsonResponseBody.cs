using Newtonsoft.Json;

namespace Bedrock.Shared.Web.Utility
{
    public class HttpJsonResponseBody
    {
        /// <summary>
        /// When instantiated and returned in a response, will be returned in the form of {body: {object}}
        /// </summary>
        [JsonProperty("body")]
        public object ResponseBody { get; set; }
    }
}

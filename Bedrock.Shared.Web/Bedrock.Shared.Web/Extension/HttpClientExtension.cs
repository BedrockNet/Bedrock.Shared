using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Utility;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bedrock.Shared.Web.Extension
{
    public static class HttpClientExtension
    {
        #region Private Properties
        private static string MediaTypeApplicationJson => StringHelper.Current.Lookup(StringMediaType.ApplicationJson);

        private static DefaultContractResolver ContractResolver => new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        private static JsonSerializerSettings SerializerSettings => new JsonSerializerSettings
        {
            ContractResolver = ContractResolver
        };
        #endregion

        #region Public Methods
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string url, T data, CancellationToken cancellationToken = default(CancellationToken))
        {
            var dataAsString = JsonConvert.SerializeObject(data, SerializerSettings);
            var content = new StringContent(dataAsString, Encoding.UTF8, MediaTypeApplicationJson);

            return httpClient.PostAsync(url, content, cancellationToken);
        }

        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient httpClient, string url, T data, CancellationToken cancellationToken = default(CancellationToken))
        {
            var dataAsString = JsonConvert.SerializeObject(data, SerializerSettings);
            var content = new StringContent(dataAsString, Encoding.UTF8, MediaTypeApplicationJson);

            return httpClient.PutAsync(url, content, cancellationToken);
        }
        #endregion
    }
}

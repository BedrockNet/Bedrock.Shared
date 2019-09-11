using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Web.Client.Configuration;
using Bedrock.Shared.Web.Client.Interface;
using Bedrock.Shared.Web.Client.Response;

using Bedrock.Shared.Web.Extension;

namespace Bedrock.Shared.Web.Client
{
    public class WebApiClient : WebApiClientBase, IWebApiClient
    {
		#region Constructors
		public WebApiClient(HttpClient client = null) : this(WebApiClientConfiguration.Default, client) { }

        public WebApiClient(WebApiClientConfiguration configuration, HttpClient client = null) : base(configuration, client) { }
        #endregion

        #region IWebApiClient Methods
        public override async Task<WebApiResponse<object>> GetAsync(string uri, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetAsync<object>(uri, cancellationToken);
        }

        public override async Task<WebApiResponse<T>> GetAsync<T>(string uri, CancellationToken cancellationToken = default(CancellationToken))
        {
			var response = await Client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
			return GetWebApiResponse<T>(response);
        }

        public override async Task<WebApiResponse<object>> PostAsync(string uri, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await PostAsync<object>(uri, cancellationToken);
        }

        public override async Task<WebApiResponse<T>> PostAsync<T>(string uri, CancellationToken cancellationToken = default(CancellationToken))
        {
			var response = await Client.PostAsync(uri, null, cancellationToken).ConfigureAwait(false);
			return GetWebApiResponse<T>(response);
        }

        public override async Task<WebApiResponse<T>> PostAsync<T>(string uri, T postData, CancellationToken cancellationToken = default(CancellationToken))
        {
			var response = await Client.PostAsJsonAsync(uri, postData, cancellationToken).ConfigureAwait(false);
			return GetWebApiResponse<T>(response);
        }

        public override async Task<WebApiResponse<T>> PostAsync<T, U>(string uri, U postData, CancellationToken cancellationToken = default(CancellationToken))
        {
			var response = await Client.PostAsJsonAsync(uri, postData, cancellationToken).ConfigureAwait(false);
			return GetWebApiResponse<T>(response);
        }

        public override async Task<WebApiResponse<T>> PutAsync<T>(string uri, T putData, CancellationToken cancellationToken = default(CancellationToken))
        {
			var response = await Client.PutAsJsonAsync(uri, putData, cancellationToken).ConfigureAwait(false);
			return GetWebApiResponse<T>(response);
        }

        public override async Task<WebApiResponse<T>> PutAsync<T, U>(string uri, U putData, CancellationToken cancellationToken = default(CancellationToken))
        {
			var response = await Client.PutAsJsonAsync(uri, putData, cancellationToken).ConfigureAwait(false);
			return GetWebApiResponse<T>(response);
        }

        public override async Task<WebApiResponse<bool>> DeleteAsync(string uri, CancellationToken cancellationToken = default(CancellationToken))
        {
			var response = await Client.DeleteAsync(uri, cancellationToken).ConfigureAwait(false);
			return GetWebApiResponse(response.IsSuccessStatusCode, response);
        }
        #endregion
    }
}

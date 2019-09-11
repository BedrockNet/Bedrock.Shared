using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Extension;

using Bedrock.Shared.Web.Client.Configuration;
using Bedrock.Shared.Web.Client.Interface;
using Bedrock.Shared.Web.Client.Response;

using Bedrock.Shared.Web.Extension;

namespace Bedrock.Shared.Web.Client
{
    public abstract class WebApiClientBase : IWebApiClient
    {
        #region Constructors
        public WebApiClientBase(WebApiClientConfiguration configuration, HttpClient client = null)
		{
			Initialize(configuration, client);
		}
        #endregion

        #region Public Properties
        public WebApiClientConfiguration Configuration { get; set; }
        #endregion

        #region Protected Properties
        protected HttpClient Client { get; private set; }
        #endregion

        #region IWebApiClient Methods
        public virtual Task<WebApiResponse<object>> GetAsync(string uri, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public virtual Task<WebApiResponse<T>> GetAsync<T>(string uri, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public virtual Task<WebApiResponse<object>> PostAsync(string uri, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public virtual Task<WebApiResponse<T>> PostAsync<T>(string uri, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public virtual Task<WebApiResponse<T>> PostAsync<T>(string uri, T postData, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public virtual Task<WebApiResponse<T>> PostAsync<T, U>(string uri, U postData, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public virtual Task<WebApiResponse<T>> PutAsync<T>(string uri, T putData, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public virtual Task<WebApiResponse<T>> PutAsync<T, U>(string uri, U putData, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public virtual Task<WebApiResponse<bool>> DeleteAsync(string uri, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
		#endregion

		#region IDisposable Methods
		public void Dispose()
		{
			Client.Dispose();
		}
		#endregion

		#region Public Methods
		public virtual T GetResponsePayload<T>(HttpResponseMessage response)
        {
            var returnValue = default(T);

            if (response != null && response.IsSuccessStatusCode && response.Content != null)
                returnValue = response.Content.ReadAsJsonAsync<T>().Result;

            return returnValue;
        }

        public virtual IEnumerable<WebApiResponseError> GetContentErrors(HttpResponseMessage response)
        {
            var returnValue = Enumerable.Empty<WebApiResponseError>();

            if (response != null && !response.IsSuccessStatusCode && response.Content != null)
                try
                {
                    returnValue = response.Content.ReadAsJsonAsync<IEnumerable<WebApiResponseError>>().Result;
                }
                catch { }

            return returnValue;
        }
        #endregion

        #region Protected Methods
        protected virtual WebApiResponse<bool> GetWebApiResponse(bool responseValue, HttpResponseMessage response)
        {
            var responseMessage = CreateResponseMessage(response);
            var errors = GetContentErrors(response);

            return WebApiResponse<bool>.Create(responseValue, responseMessage, errors);
        }

        protected virtual WebApiResponse<T> GetWebApiResponse<T>(HttpResponseMessage response)
        {
            var responseValue = GetResponsePayload<T>(response);
            var responseMessage = CreateResponseMessage(response);
            var errors = GetContentErrors(response);

            return WebApiResponse<T>.Create(responseValue, responseMessage, errors);
        }

        protected virtual WebApiResponseMessage CreateResponseMessage(HttpResponseMessage response)
        {
            return WebApiResponseMessage.Create
            (
                response.IsSuccessStatusCode,
                response.StatusCode,
                response.ReasonPhrase,
                response.Content != null ? response?.Content?.Headers?.ContentType?.ToString() : null,
                response.Headers.Location
            );
        }
		#endregion

		#region Private Methods
		private void Initialize(WebApiClientConfiguration configuration, HttpClient client)
		{
            configuration.ConfigurationChanged += OnConfigurationChanged;

			Configuration = configuration;
			CreateClient(client);
		}

        private void CreateClient(HttpClient client)
		{
            Client = client ?? new HttpClient();
            ApplyConfiguration();
        }

        private void ApplyConfiguration(ConfigurationChangedEventArgs e = null)
        {
            SetBaseAddress(Client, e);

            UpdateDefaultRequestHeaders(Client, e);
            UpdateAcceptHeaders(Client, e);
            UpdateAcceptHeaderEncodings(Client, e);

            SetBearerToken(Client, e);
        }

        private void SetBaseAddress(HttpClient httpClient, ConfigurationChangedEventArgs e = null)
        {
            if((e == null || e.PropertyChanged == nameof(WebApiClientConfiguration.BaseAddress)) && !string.IsNullOrWhiteSpace(Configuration.BaseAddress))
                httpClient.BaseAddress = new Uri(Configuration.BaseAddress);
        }

        private void UpdateDefaultRequestHeaders(HttpClient httpClient, ConfigurationChangedEventArgs e = null)
        {
            if ((e == null || e.PropertyChanged == nameof(WebApiClientConfiguration.DefaultRequestHeaders)) && Configuration.DefaultRequestHeaders != null)
            {
                httpClient.DefaultRequestHeaders.Clear();
                Configuration.DefaultRequestHeaders.Each(drh => httpClient.DefaultRequestHeaders.Add(drh.Key, drh.Value));

                UpdateAcceptHeaders(Client, null);
                UpdateAcceptHeaderEncodings(Client, null);
            }
        }

        private void UpdateAcceptHeaders(HttpClient httpClient, ConfigurationChangedEventArgs e = null)
        {
            if ((e == null || e.PropertyChanged == nameof(WebApiClientConfiguration.AcceptHeaders)) && Configuration.AcceptHeaders != null)
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                Configuration.AcceptHeaders.Each(ah => httpClient.DefaultRequestHeaders.Accept.Add(ah));
            }
        }

        private void UpdateAcceptHeaderEncodings(HttpClient httpClient, ConfigurationChangedEventArgs e = null)
        {
            if ((e == null || e.PropertyChanged == nameof(WebApiClientConfiguration.AcceptHeaderEncodings)) && Configuration.AcceptHeaderEncodings != null)
            {
                httpClient.DefaultRequestHeaders.AcceptEncoding.Clear();
                Configuration.AcceptHeaderEncodings.Each(ahe => httpClient.DefaultRequestHeaders.AcceptEncoding.Add(ahe));
            }
        }

        private void SetBearerToken(HttpClient httpClient, ConfigurationChangedEventArgs e = null)
        {
            if (e == null || e.PropertyChanged == nameof(WebApiClientConfiguration.BearerToken))
                SetBearerToken(httpClient, Configuration.BearerToken);
        }

        private void SetBearerToken(HttpClient httpClient, string token)
        {
            SetToken(httpClient, "Bearer", token);
        }

        private void SetToken(HttpClient httpClient, string scheme, string token)
        {
            if (!string.IsNullOrWhiteSpace(scheme) && !string.IsNullOrWhiteSpace(token))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
            else
                httpClient.DefaultRequestHeaders.Authorization = null;
        }

        private void OnConfigurationChanged(object sender, ConfigurationChangedEventArgs e)
        {
            ApplyConfiguration(e);
        }
        #endregion
    }
}

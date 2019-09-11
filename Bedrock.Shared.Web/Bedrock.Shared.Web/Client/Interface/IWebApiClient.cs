using System;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Web.Client.Configuration;
using Bedrock.Shared.Web.Client.Response;

namespace Bedrock.Shared.Web.Client.Interface
{
    public interface IWebApiClient : IDisposable
    {
        #region Properties
        WebApiClientConfiguration Configuration { get; set; }
        #endregion

        #region Methods
        Task<WebApiResponse<object>> GetAsync(string uri, CancellationToken cancellationToken = default(CancellationToken));

        Task<WebApiResponse<T>> GetAsync<T>(string uri, CancellationToken cancellationToken = default(CancellationToken));

        Task<WebApiResponse<object>> PostAsync(string uri, CancellationToken cancellationToken = default(CancellationToken));

        Task<WebApiResponse<T>> PostAsync<T>(string uri, CancellationToken cancellationToken = default(CancellationToken));

        Task<WebApiResponse<T>> PostAsync<T>(string uri, T postData, CancellationToken cancellationToken = default(CancellationToken));

        Task<WebApiResponse<T>> PostAsync<T, U>(string uri, U postData, CancellationToken cancellationToken = default(CancellationToken));

        Task<WebApiResponse<T>> PutAsync<T>(string uri, T putData, CancellationToken cancellationToken = default(CancellationToken));

        Task<WebApiResponse<T>> PutAsync<T, U>(string uri, U putData, CancellationToken cancellationToken = default(CancellationToken));

        Task<WebApiResponse<bool>> DeleteAsync(string uri, CancellationToken cancellationToken = default(CancellationToken));
        #endregion
    }
}

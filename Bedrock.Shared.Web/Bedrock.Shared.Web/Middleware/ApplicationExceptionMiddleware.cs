using System.Text;
using System.Threading.Tasks;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Log.Interface;
using Bedrock.Shared.Utility;

using Bedrock.Shared.Web.Middleware.Options;
using Bedrock.Shared.Web.Utility;

using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

using Newtonsoft.Json;

namespace Bedrock.Shared.Web.Middleware
{
    internal class ApplicationExceptionMiddleware
    {
        #region Fields
        private readonly RequestDelegate _next;
        private readonly ApplicationExceptionMiddlewareOptions _options;
        #endregion

        #region Constructors
        public ApplicationExceptionMiddleware(RequestDelegate next, ApplicationExceptionMiddlewareOptions options)
        {
            _next = next;
            _options = options;
        }
        #endregion

        #region Public Methods
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (System.Exception exception)
            {
                var content = !string.IsNullOrWhiteSpace(_options.ErrorMessage) ? _options.ErrorMessage : StringHelper.Current.Lookup(StringError.UnhandledException);
                var responseBody = new HttpJsonResponseBody { ResponseBody = content };
                var responseText = JsonConvert.SerializeObject(responseBody);

                var logger = context.RequestServices.GetService(typeof(ILogger)) as ILogger;
                logger.Error(exception, content);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = new MediaTypeHeaderValue(StringHelper.Current.Lookup(StringMediaType.ApplicationJson)).ToString();

                await context.Response.WriteAsync(responseText, Encoding.UTF8);
            }
        }
        #endregion
    }
}

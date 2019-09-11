using System.Text;
using System.Threading.Tasks;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Log.Interface;
using Bedrock.Shared.Utility;

using Bedrock.Shared.Web.Exception;
using Bedrock.Shared.Web.Utility;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Bedrock.Shared.Web.Middleware
{
    internal class HttpExceptionMiddleware
    {
        #region Fields
        private readonly RequestDelegate _next;
        #endregion

        #region Constructors
        public HttpExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        #endregion

        #region Public Methods
        public async Task Invoke(HttpContext context)
        {
            var logger = context.RequestServices.GetService(typeof(ILogger)) as ILogger;

            try
            {
                await _next.Invoke(context);
            }
            catch (HttpException httpException)
            {
                context.Response.StatusCode = httpException.StatusCode;
                context.Response.ContentType = new MediaTypeHeaderValue(StringHelper.Current.Lookup(StringMediaType.ApplicationJson)).ToString();

                if(httpException.Content != null)
                {
                    var content = JsonConvert.SerializeObject(httpException.Content, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                    logger.Error(httpException, content);

                    await context.Response.WriteAsync(content, Encoding.UTF8);
                }
            }
            catch (System.Exception exception)
            {
                var content = StringHelper.Current.Lookup(StringError.UnhandledException);
                var responseBody = new HttpJsonResponseBody { ResponseBody = content };
                var responseText = JsonConvert.SerializeObject(responseBody);

                logger.Error(exception, content);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = new MediaTypeHeaderValue(StringHelper.Current.Lookup(StringMediaType.ApplicationJson)).ToString();

                await context.Response.WriteAsync(responseText, Encoding.UTF8);
            }
        }
        #endregion
    }
}

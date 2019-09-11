using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Bedrock.Shared.Web.Middleware
{
    public class EndRequestMiddleware
    {
        #region Member Fields
        private readonly RequestDelegate _next;
        #endregion

        #region Constructors
        public EndRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        #endregion

        #region Public Methods
        public async Task Invoke(HttpContext context)
        {
            await _next.Invoke(context);
        }
        #endregion
    }
}

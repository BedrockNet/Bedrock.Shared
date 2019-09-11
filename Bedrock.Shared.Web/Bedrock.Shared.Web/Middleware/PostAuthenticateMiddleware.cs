using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Security.Extension;
using Bedrock.Shared.Security.Identity;
using Bedrock.Shared.Security.Interface;

using Bedrock.Shared.Web.Middleware.Options;
using Microsoft.AspNetCore.Http;

using SecurityModel = Bedrock.Shared.Security.Model;

namespace Bedrock.Shared.Web.Middleware
{
    public class PostAuthenticateMiddleware
    {
        #region Fields
        private readonly PostAuthenticationMiddlewareOptions _options;
        private readonly RequestDelegate _next;
        #endregion

        #region Constructors
        public PostAuthenticateMiddleware(RequestDelegate next, PostAuthenticationMiddlewareOptions options)
        {
            _options = options;
            _next = next;
        }
        #endregion

        #region Properties
        public IBedrockUser BedrockUser { get; set; }
        #endregion

        #region Public Methods
        public async Task Invoke(HttpContext context)
        {
            await PostAuthenticate(context);
            await _next.Invoke(context);
        }
        #endregion

        #region Protected Methods
        protected virtual async Task PostAuthenticate(HttpContext context)
        {
            var claimsPrincipal = context.User.Transform(_options.ClaimType);
            BedrockUser = SecurityModel.BedrockUser.CreateFromPrincipal(claimsPrincipal, _options.ClaimType);

            var bedrockIdentity = new BedrockIdentity(BedrockUser, (ClaimsIdentity)context.User.Identity);
            var bedrockPrincipal = new BedrockPrincipal(BedrockUser, bedrockIdentity);

            bedrockPrincipal.AddIdentity(bedrockIdentity);

            context.User = bedrockPrincipal;
            Thread.CurrentPrincipal = bedrockPrincipal;
        }
        #endregion
    }
}

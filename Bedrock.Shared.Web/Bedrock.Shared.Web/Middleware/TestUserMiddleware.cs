using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Extension;

using Bedrock.Shared.Security.Identity;
using Bedrock.Shared.Security.Model;

using Bedrock.Shared.Utility;

using Bedrock.Shared.Web.Extension;
using Bedrock.Shared.Web.Middleware.Options;

using Microsoft.AspNetCore.Http;

namespace Bedrock.Shared.Web.Middleware
{
    public class TestUserMiddleware
    {
        #region Fields
        private readonly TestUserMiddlewareOptions _options;
        private readonly RequestDelegate _next;
        #endregion

        #region Constructors
        public TestUserMiddleware(RequestDelegate next, TestUserMiddlewareOptions options)
        {
            _options = options;
            _next = next;
        }
        #endregion

        #region Public Methods
        public async Task Invoke(HttpContext context)
        {
            SetUser(context);
            await _next.Invoke(context);
        }
        #endregion

        #region Private Methods
        private void SetUser(HttpContext context)
        {
            var displayName = context.GetUsername();

            var user = new BedrockUser
            {
                UserId = BedrockUser.Default.UserId,
                DisplayName = displayName,
                UserType = BedrockUser.Default.UserType
            };

            context.User = new BedrockPrincipal(user, new BedrockIdentity(user));
            context.User.AddIdentity((ClaimsIdentity)context.User.Identity);

            var type = StringHelper.Current.Lookup(StringClaimType.Role);
            var userName = StringHelper.Current.Lookup(StringClaimType.PreferredUserName);
            var subject = StringHelper.Current.Lookup(StringClaimType.Subject);

            context.User.Identities.First().AddClaim(new Claim(userName, context.User.Identity.Name));
            context.User.Identities.First().AddClaim(new Claim(subject, context.User.Identity.Name));

            _options.Applications.Each(a => context.User.Identities.First().AddClaim(new Claim(type, a.ToString())));
        }
        #endregion
    }
}

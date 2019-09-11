using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Log.Interface;

using Bedrock.Shared.Security.Extension;
using Bedrock.Shared.Security.Identity;
using Bedrock.Shared.Security.Interface;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

using SecurityModel = Bedrock.Shared.Security.Model;

namespace Bedrock.Shared.Web.Security
{
    public class BedrockBearerEvents : JwtBearerEvents
    {
        #region Constructors
        public BedrockBearerEvents(IClaimType claimType)
        {
            ClaimType = claimType;
        }
        #endregion

        #region Public Properties
        public IClaimType ClaimType { get; private set; }

        public ClaimsPrincipal ClaimsPrincipal { get; set; }

        public IBedrockUser BedrockUser { get; set; }
        #endregion

        #region JwtBearerEvents Methods
        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILogger>();
            logger.Error(context.Exception, "Authentication Failed");

            return base.AuthenticationFailed(context);
        }

        public override Task TokenValidated(TokenValidatedContext context)
        {
            ClaimsPrincipal = context.Principal.Transform(ClaimType);
            BedrockUser = SecurityModel.BedrockUser.CreateFromPrincipal(ClaimsPrincipal, ClaimType);

            var bedrockIdentity = new BedrockIdentity(BedrockUser);
            var bedrockPrincipal = new BedrockPrincipal(BedrockUser, bedrockIdentity);

            bedrockIdentity.AddClaims(ClaimsPrincipal.Claims);
            bedrockPrincipal.AddIdentity(bedrockIdentity);

            context.Principal = bedrockPrincipal;
            Thread.CurrentPrincipal = bedrockPrincipal;
            ClaimsPrincipal = bedrockPrincipal;

            return base.TokenValidated(context);
        }
        #endregion
    }
}

using System;
using System.Threading.Tasks;

using Bedrock.Shared.Security.Identity;
using Bedrock.Shared.Security.Interface;
using Bedrock.Shared.Security.Model;

using Bedrock.Shared.Web.Controller;
using Bedrock.Shared.Web.Extension;

using Microsoft.AspNetCore.Mvc.Filters;
using SessionInterface = Bedrock.Shared.Session.Interface;

namespace Bedrock.Shared.Web.Filter
{
    public class SetSessionUserAttribute : ActionFilterAttribute
    {
        #region Protected Methods
        protected SessionInterface.ISession Session { get; set; }
        #endregion

        #region ActionFilterAttribute Methods
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            SetSession(context);
            Session.SetUser(GetUser(context), context.HttpContext.User);

            return base.OnActionExecutionAsync(context, next);
        }
        #endregion

        #region Private Methods
        private IBedrockUser GetUser(ActionExecutingContext context)
        {
            var returnValue = default(IBedrockUser);
            var bedrockPrincipal = context.HttpContext.User as BedrockPrincipal;

            if(bedrockPrincipal != null)
                returnValue = bedrockPrincipal.BedrockUser;
            else
            {
                returnValue = new BedrockUser
                {
                    UserId = BedrockUser.Default.UserId,
                    DisplayName = context.HttpContext.GetUsername(),
                    UserType = BedrockUser.Default.UserType
                };
            }

            return returnValue;
        }

        private void SetSession(ActionExecutingContext context)
        {
            var controller = context.Controller as ControllerBase;

            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            Session = controller.CurrentSession;

            if (Session == null)
                throw new ArgumentNullException(nameof(Session));
        }
        #endregion
    }
}

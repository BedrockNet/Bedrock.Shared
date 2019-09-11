using System;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Session.Interface;
using Bedrock.Shared.Web.Controller;
using Bedrock.Shared.Web.Security;

using Microsoft.AspNetCore.Mvc.Filters;

namespace Bedrock.Shared.Web.Filter
{
	public class AuthorizeAttribute : ActionFilterAttribute
	{
		#region Protected Methods
		protected ISession Session { get; set; }
        #endregion

        #region ActionFilterAttribute Methods
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			SetSession(context);

			var authenticator = (BedrockAuthenticator)context.HttpContext.RequestServices.GetService(typeof(IAuthenticator));
			authenticator.Session = Session;

			Thread.CurrentPrincipal = authenticator.Authenticate(context.HttpContext);

			return base.OnActionExecutionAsync(context, next);
		}
		#endregion

		#region Private Methods
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

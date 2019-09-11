using System;
using Bedrock.Shared.Web.ActionResult;

using Microsoft.AspNetCore.Mvc;
using Mvc = Microsoft.AspNetCore.Mvc;

namespace Bedrock.Shared.Web.Extension
{
    public static class ControllerExtension
    {
        #region Public Methods
        public static IActionResult AccessDenied(this Mvc.Controller controller)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return new AccessDeniedResult(controller.User);
        }
        #endregion
    }
}

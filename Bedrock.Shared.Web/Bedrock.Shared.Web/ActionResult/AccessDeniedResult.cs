using System.Net;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;

namespace Bedrock.Shared.Web.ActionResult
{
    public class AccessDeniedResult : StatusCodeResult
    {
        #region Constructors
        public AccessDeniedResult(ClaimsPrincipal claimsPrincipal) : base(GetCode(claimsPrincipal)) { }
        #endregion

        #region Public Methods
        public static int GetCode(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null && claimsPrincipal.Identity != null && claimsPrincipal.Identity.IsAuthenticated)
                return (int)HttpStatusCode.Forbidden;
            else
                return (int)HttpStatusCode.Unauthorized;
        }
        #endregion
    }
}

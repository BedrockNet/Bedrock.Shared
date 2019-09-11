using System.Net;
using System.Security;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Extension;
using Bedrock.Shared.Utility;

using Microsoft.AspNetCore.Mvc.Filters;

namespace Bedrock.Shared.Web.Filter.Exception
{
    public class SecurityExceptionFilterAttribute : ExceptionFilterBase
    {
        #region ExceptionFilterAttribute Members
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception.GetInnermostException() is SecurityException)
            {
                var reasonPhrase = StringHelper.Current.Lookup(StringError.SecurityFilter);
                context.Result = GetErrorResponseMessage(HttpStatusCode.Forbidden, reasonPhrase, context);
            }
        }
        #endregion
    }
}
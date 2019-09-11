using System.Collections.Generic;
using System.Net;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Extension;
using Bedrock.Shared.Utility;

using Microsoft.AspNetCore.Mvc.Filters;

namespace Bedrock.Shared.Web.Filter.Exception
{
    public class KeyNotFoundExceptionFilterAttribute : ExceptionFilterBase
    {
        #region ExceptionFilterAttribute Members
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception.GetInnermostException() is KeyNotFoundException)
            {
                var reasonPhrase = StringHelper.Current.Lookup(StringError.KeyNotFoundFilter);
                context.Result = GetErrorResponseMessage(HttpStatusCode.BadRequest, reasonPhrase, context);
            }
        }
        #endregion
    }
}
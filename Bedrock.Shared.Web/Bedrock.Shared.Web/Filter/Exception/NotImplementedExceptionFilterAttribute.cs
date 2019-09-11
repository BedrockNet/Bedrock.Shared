using System;
using System.Net;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Extension;
using Bedrock.Shared.Utility;

using Microsoft.AspNetCore.Mvc.Filters;

namespace Bedrock.Shared.Web.Filter.Exception
{
    public class NotImplementedExceptionFilterAttribute : ExceptionFilterBase
    {
        #region ExceptionFilterAttribute Members
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception.GetInnermostException() is NotImplementedException)
            {
                var reasonPhrase = StringHelper.Current.Lookup(StringError.NotImplementedFilter);
                context.Result  = GetErrorResponseMessage(HttpStatusCode.NotImplemented, reasonPhrase, context);
            }
        }
        #endregion
    }
}
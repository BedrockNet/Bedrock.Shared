using System.Collections.Generic;
using System.Net;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Extension;
using Bedrock.Shared.Utility;
using Bedrock.Shared.Web.Client.Response;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bedrock.Shared.Web.Filter.Exception
{
    public abstract class ExceptionFilterBase : ExceptionFilterAttribute
    {
        #region Constructors
        public ExceptionFilterBase()
        {
            Errors = new List<WebApiResponseError>();
        }
        #endregion

        #region Private Properties
        private List<WebApiResponseError> Errors { get; set; }
        #endregion

        #region Protected Methods
        protected IActionResult GetErrorResponseMessage(HttpStatusCode statusCode, string reasonPhrase, ExceptionContext context)
        {
            var content = GetErrors(reasonPhrase, context);
            return new BadRequestObjectResult(content);
        }

        protected virtual IEnumerable<WebApiResponseError> GetErrors(string reasonPhrase, ExceptionContext context)
        {
            var reasonPhraseMessageKey = StringHelper.Current.Lookup(StringApplication.ReasonPhrase);
            var exceptionMessageKey = StringHelper.Current.Lookup(StringApplication.ExceptionMessage);
            var methodNameKey = StringHelper.Current.Lookup(StringApplication.MethodName);

            var innermostException = context.Exception.GetInnermostException();
            var message = innermostException.Message;

            var methodName = innermostException.TargetSite != null ?
                                innermostException.TargetSite.DeclaringType.Name :
                                    context.Exception.TargetSite != null ?
                                        context.Exception.TargetSite.DeclaringType.Name : null;

            AddError(reasonPhraseMessageKey, reasonPhrase);
            AddError(exceptionMessageKey, message);
            AddError(methodNameKey, methodName);

            return Errors;
        }

        protected virtual void AddError(string key, string value)
        {
            Errors.Add(WebApiResponseError.CreateInstance(key, value));
        }
        #endregion
    }
}

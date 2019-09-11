using System.Data.SqlClient;
using System.Net;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Extension;
using Bedrock.Shared.Utility;

using Microsoft.AspNetCore.Mvc.Filters;

namespace Bedrock.Shared.Web.Filter.Exception
{
    public class SqlExceptionFilterAttribute : ExceptionFilterBase
    {
        #region ExceptionFilterAttribute Members
        public override void OnException(ExceptionContext context)
        {
            var sqlException = context.Exception.GetInnermostException() as SqlException;

            if (sqlException != null)
            {
                var reasonPhrase = StringHelper.Current.Lookup(StringError.SqlFilter);

                if (sqlException.Number > 50000)
                    context.Result = GetErrorResponseMessage(HttpStatusCode.BadRequest, reasonPhrase, context);
                else
                    context.Result = GetErrorResponseMessage(HttpStatusCode.InternalServerError, reasonPhrase, context);
            }
        }
        #endregion
    }
}

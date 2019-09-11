using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Bedrock.Shared.Web.Api.Swagger
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        #region IOperationFilter Methods
        public void Apply(Operation operation, OperationFilterContext context)
        {
            context.ApiDescription.TryGetMethodInfo(out var methodInfo);

            if (methodInfo == null)
                return;

            var hasAuthorizeAttribute = false;

            if (methodInfo.MemberType == MemberTypes.Method)
            {
                hasAuthorizeAttribute = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

                if (hasAuthorizeAttribute)
                    hasAuthorizeAttribute = !methodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
                else
                    hasAuthorizeAttribute = methodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
            }

            if (!hasAuthorizeAttribute)
                return;

            operation.Responses.Add(StatusCodes.Status401Unauthorized.ToString(), new Response { Description = HttpStatusCode.Unauthorized.ToString() });
            operation.Responses.Add(StatusCodes.Status403Forbidden.ToString(), new Response { Description = HttpStatusCode.Forbidden.ToString() });

            operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
            operation.Security.Add(new Dictionary<string, IEnumerable<string>>
            {
                { "Bearer", new string[] { } }
            });
        }
        #endregion
    }
}

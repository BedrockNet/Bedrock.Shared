using System.Security.Principal;
using Microsoft.AspNetCore.Http;

namespace Bedrock.Shared.Web.Security
{
	public interface IAuthenticator
	{
		#region Methods
		IPrincipal Authenticate(HttpContext httpContext);
		#endregion
	}
}

using System.Security.Principal;

using Bedrock.Shared.Security.Identity;
using Bedrock.Shared.Security.Model;

using Bedrock.Shared.Web.Extension;

using Microsoft.AspNetCore.Http;
using SharedSession = Bedrock.Shared.Session.Interface;

namespace Bedrock.Shared.Web.Security
{
	public abstract class AuthenticatorBase : IAuthenticator
	{
		#region Public Properties
		public SharedSession.ISession Session { get; set; }
		#endregion

		#region IAuthenticator Methods
		public virtual IPrincipal Authenticate(HttpContext httpContext)
		{
			var windowsIdentity = WindowsIdentity.GetCurrent();

			if (windowsIdentity == null || !windowsIdentity.IsAuthenticated)
				return IncompletePrincipal.Instance;

			if (windowsIdentity.User == null || windowsIdentity.Groups == null)
				return IncompletePrincipal.Instance;

			var username = httpContext.GetUsername();

			if (string.IsNullOrWhiteSpace(username))
				return IncompletePrincipal.Instance;

			var user = GetUser(username);
			Session.SetUser(user);

			if (httpContext != null)
				httpContext.User = Session.Principal;

			return httpContext.User;
		}
		#endregion

		#region Protected Methods
		protected virtual BedrockUser GetUser(string displayName)
		{
			return new BedrockUser
			{
				UserId = BedrockUser.Default.UserId,
				DisplayName = displayName,
				UserType = BedrockUser.Default.UserType
			};
		}
		#endregion
	}
}

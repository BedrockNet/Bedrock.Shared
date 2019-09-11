using System.Security.Principal;

namespace Bedrock.Shared.Security.Identity
{
	public class IncompleteIdentity : IIdentity
	{
		#region IIdentity Members
		public string Name { get; private set; }

		public string AuthenticationType { get; private set; }

		public bool IsAuthenticated { get; private set; }
		#endregion
	}
}

using System.Security.Principal;

namespace Bedrock.Shared.Security.Identity
{
	public class IncompletePrincipal : IPrincipal
	{
		#region Constructors
		public IncompletePrincipal()
		{
			Identity = new IncompleteIdentity();
		}
		#endregion

		#region Properties
		public static IPrincipal Instance => new IncompletePrincipal();
		#endregion

		#region IPrincipal Members
		public bool IsInRole(string role)
		{
			return false;
		}

		public IIdentity Identity { get; private set; }
		#endregion
	}
}

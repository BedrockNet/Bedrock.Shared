using System.Security.Claims;
using System.Security.Principal;

using Bedrock.Shared.Security.Interface;

namespace Bedrock.Shared.Security.Identity
{
	public class BedrockPrincipal : ClaimsPrincipal
	{
		#region Fields
		private readonly BedrockIdentity _identity;
		#endregion

		#region Constructors
		public BedrockPrincipal(IBedrockUser bedrockUser, BedrockIdentity identity)
		{
            BedrockUser = bedrockUser;
            _identity = identity;
		}
        #endregion

        #region Public Properties
        public IBedrockUser BedrockUser { get; set; }
        #endregion

        #region IPrincipal Methods
        public override IIdentity Identity
		{
			get { return _identity; }
		}

		public override bool IsInRole(string role)
		{
			return true;
		}
        #endregion
    }
}

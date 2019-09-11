using System.Security.Claims;
using Bedrock.Shared.Security.Interface;

namespace Bedrock.Shared.Security.Identity
{
	public class BedrockIdentity : ClaimsIdentity
	{
		#region Fields
		private string _name;
        #endregion

        #region Constructors
        public BedrockIdentity(IBedrockUser user) : this(user.DisplayName) { }

        public BedrockIdentity(IBedrockUser user, ClaimsIdentity claimsIdentity) : this(user.DisplayName, claimsIdentity) { }

        public BedrockIdentity(string name, ClaimsIdentity claimsIdentity) : base(claimsIdentity)
        {
            _name = name;
        }

        public BedrockIdentity(string name)
        {
            _name = name;
        }
        #endregion

        #region IIdentity Members
        public override string Name
		{
			get { return _name; }
		}
		#endregion
	}
}

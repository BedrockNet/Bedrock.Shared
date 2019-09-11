using System.Collections.Generic;

namespace Bedrock.Shared.Security.Model
{
    public class BedrockUserModel
    {
        #region Constructors
        public BedrockUserModel()
        {
            Enabled = true;
            Claims = new List<BedrockClaimModel>();
        }
        #endregion

        #region Properties
        public string Subject { get; set; }

        public bool Enabled { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Provider { get; set; }

        public string ProviderId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NameFormatted
        {
            get { return string.Format("{0}, {1}", LastName, FirstName); }
        }

        public string EmailAddress { get; set; }

        public IEnumerable<BedrockClaimModel> Claims { get; set; }
        #endregion
    }
}

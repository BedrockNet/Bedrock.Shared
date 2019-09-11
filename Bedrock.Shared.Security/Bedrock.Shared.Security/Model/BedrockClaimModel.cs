namespace Bedrock.Shared.Security.Model
{
    public class BedrockClaimModel
    {
        #region Properties
        public string Issuer { get; set; }

        public string OriginalIssuer { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public string ValueType { get; set; }

        public string ResourceType { get; set; }

        public string Application { get; set; }
        #endregion
    }
}

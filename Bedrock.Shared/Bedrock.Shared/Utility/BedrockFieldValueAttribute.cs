namespace Bedrock.Shared.Utility
{
    public class BedrockFieldValueAttribute : System.Attribute
    {
        #region Constructors
        public BedrockFieldValueAttribute(string value)
        {
            Value = value;
        }
        #endregion

        #region Public Properties
        public string Value { get; private set; }
        #endregion
    }
}

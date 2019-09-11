namespace Bedrock.Shared.Web.Extension
{
    public static class StringExtension
    {
        #region Public Methods
        public static string PrepareForLog(this string value)
        {
            return value.Replace("{", "{{").Replace("}", "}}");
        }
        #endregion
    }
}

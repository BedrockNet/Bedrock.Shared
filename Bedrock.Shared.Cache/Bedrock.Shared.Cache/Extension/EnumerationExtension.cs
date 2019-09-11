using Bedrock.Shared.Cache.Enumeration;

namespace Bedrock.Shared.Cache.Extension
{
    public static class EnumerationExtension
    {
        #region Public Methods
        public static string ToSymbolString(this WildCard wildCard)
        {
            var returnValue = string.Empty;

            switch (wildCard)
            {
                case WildCard.Asterick:
                    {
                        returnValue = "*";
                        break;
                    }

                default:
                    {
                        returnValue = "?";
                        break;
                    }
            }

            return returnValue;
        }
        #endregion
    }
}

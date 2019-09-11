using System;

namespace Bedrock.Shared.Extension
{
    public static class StringExtension
    {
        #region Public Methods
        public static bool HasValue(this string value)
        {
            return value != null && value.Length > 0;
        }

        public static string ToUpperFirstLetter(this string value)
        {
            return char.ToUpperInvariant(value[0]) + value.Substring(1);
        }

        public static string ToLowerFirstLetter(this string value)
        {
            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public static bool IsValidUri(this string value)
        {
            Uri uriResult;
            return Uri.TryCreate(value, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public static T ToEnum<T>(this string value, bool isIgnoreCase)
        {
            return (T)Enum.Parse(typeof(T), value, isIgnoreCase);
        }
        #endregion
    }
}

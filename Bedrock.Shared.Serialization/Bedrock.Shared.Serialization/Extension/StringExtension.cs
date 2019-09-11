using SerializationEnumeration = Bedrock.Shared.Serialization.Enumeration;

namespace Bedrock.Shared.Serialization.Extension
{
    public static class StringExtension
    {
        #region Public Methods
        public static byte[] GetBytes(this string value, SerializationEnumeration.Encoding encoding)
        {
            return encoding.GetSystemType().GetBytes(value);
        }
        #endregion
    }
}

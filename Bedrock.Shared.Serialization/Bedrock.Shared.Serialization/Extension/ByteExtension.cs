using SerializationEnumeration = Bedrock.Shared.Serialization.Enumeration;

namespace Bedrock.Shared.Serialization.Extension
{
    public static class ByteExtension
    {
        #region Public Methods
        public static string GetString(this byte[] value, SerializationEnumeration.Encoding encoding)
        {
            return encoding.GetSystemType().GetString(value);
        }
        #endregion
    }
}

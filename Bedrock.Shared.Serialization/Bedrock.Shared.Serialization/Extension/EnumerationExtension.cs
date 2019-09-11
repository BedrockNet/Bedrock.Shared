using System.Text;
using SerializationEnumeration = Bedrock.Shared.Serialization.Enumeration;

namespace Bedrock.Shared.Serialization.Extension
{
    public static class EnumerationExtension
    {
        #region Public Methods
        public static Encoding GetSystemType(this SerializationEnumeration.Encoding encoding)
        {
            return Encoding.GetEncoding((int)encoding);
        }
        #endregion
    }
}

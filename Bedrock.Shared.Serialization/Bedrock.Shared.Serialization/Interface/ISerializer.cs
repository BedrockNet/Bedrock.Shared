using Bedrock.Shared.Serialization.Enumeration;

namespace Bedrock.Shared.Serialization.Interface
{
    public interface ISerializer
    {
        #region Properties
        Serializer SerializerType { get; }
        #endregion

        #region Methods
        byte[] Serialize<T>(T value);

        byte[] Serialize<T>(T value, Encoding encoding);

        string SerializeToString<T>(T value);

        string SerializeToString<T>(T value, Encoding encoding);

        T Deserialize<T>(byte[] bytes);

        T Deserialize<T>(byte[] bytes, Encoding encoding);

        T DeserializeFromString<T>(string value);

        T DeserializeFromString<T>(string value, Encoding encoding);

        T Clone<T>(T value);
        #endregion
    }
}

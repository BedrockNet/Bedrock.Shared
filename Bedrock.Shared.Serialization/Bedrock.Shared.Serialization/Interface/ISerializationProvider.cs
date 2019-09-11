using Bedrock.Shared.Serialization.Enumeration;

namespace Bedrock.Shared.Serialization.Interface
{
    public interface ISerializationProvider
    {
        #region Methods
        byte[] Serialize<T>(T value, Serializer serializerType);

        byte[] Serialize<T>(T value, Serializer serializerType, Encoding encoding);

        string SerializeToString<T>(T value, Serializer serializerType);

        string SerializeToString<T>(T value, Serializer serializerType, Encoding encoding);

        T Deserialize<T>(byte[] bytes, Serializer serializerType);

        T Deserialize<T>(byte[] bytes, Serializer serializerType, Encoding encoding);

		T DeserializeFromString<T>(string value, Serializer serializerType);

		T DeserializeFromString<T>(string value, Serializer serializerType, Encoding encoding);

        T Clone<T>(T value, Serializer serializerType);
        #endregion
    }
}

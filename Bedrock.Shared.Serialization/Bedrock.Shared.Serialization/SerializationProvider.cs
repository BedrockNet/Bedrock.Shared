using System.Collections.Generic;
using System.Linq;

using Bedrock.Shared.Serialization.Enumeration;
using Bedrock.Shared.Serialization.Interface;

namespace Bedrock.Shared.Serialization
{
    public class SerializationProvider : ISerializationProvider
    {
        #region Constructors
        public SerializationProvider(ISerializer[] serializers)
        {
            Serializers = serializers;
        }
        #endregion

        #region Properties
        protected IEnumerable<ISerializer> Serializers { get; set; }
        #endregion

        #region ISerializationProvider Methods
        public byte[] Serialize<T>(T value, Serializer serializerType)
        {
            return GetSerializer(serializerType).Serialize(value);
        }

        public byte[] Serialize<T>(T value, Serializer serializerType, Encoding encoding)
        {
            return GetSerializer(serializerType).Serialize(value, encoding);
        }

        public string SerializeToString<T>(T value, Serializer serializerType)
        {
            return GetSerializer(serializerType).SerializeToString(value);
        }

        public string SerializeToString<T>(T value, Serializer serializerType, Encoding encoding)
        {
            return GetSerializer(serializerType).SerializeToString(value, encoding);
        }

        public T Deserialize<T>(byte[] bytes, Serializer serializerType)
        {
            return GetSerializer(serializerType).Deserialize<T>(bytes);
        }

        public T Deserialize<T>(byte[] bytes, Serializer serializerType, Encoding encoding)
        {
            return GetSerializer(serializerType).Deserialize<T>(bytes, encoding);
        }

        public T DeserializeFromString<T>(string value, Serializer serializerType)
        {
            return GetSerializer(serializerType).DeserializeFromString<T>(value);
        }

        public T DeserializeFromString<T>(string value, Serializer serializerType, Encoding encoding)
        {
            return GetSerializer(serializerType).DeserializeFromString<T>(value, encoding);
        }

        public T Clone<T>(T value, Serializer serializerType)
        {
            return GetSerializer(serializerType).Clone(value);
        }
        #endregion

        #region Private Methods
        private ISerializer GetSerializer(Serializer serializerType)
        {
            return Serializers.First(s => s.SerializerType == serializerType);
        }
        #endregion
    }
}

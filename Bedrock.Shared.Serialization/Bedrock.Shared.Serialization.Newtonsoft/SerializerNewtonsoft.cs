using System;
using System.IO;

using Bedrock.Shared.Extension;

using Bedrock.Shared.Serialization.Enumeration;
using Bedrock.Shared.Serialization.Extension;
using Bedrock.Shared.Serialization.Newtonsoft.Interface;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Serialization;

namespace Bedrock.Shared.Serialization.Newtonsoft
{
    public class SerializerNewtonsoft : SerializerBase, ISerializerNewtonsoft
    {
        #region ISerializer Properties
        public override Serializer SerializerType { get { return Serializer.Json; } }
        #endregion

        #region Private Properties
        private static DefaultContractResolver ContractResolver => new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        private static JsonSerializerSettings SerializerSettings => new JsonSerializerSettings
        {
            ContractResolver = ContractResolver
        };
        #endregion

        #region Public Methods
        public static byte[] SerializeStatic<T>(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var serializedValue = SerializeToStringStatic(value);
            return serializedValue.GetBytes(Encoding.UTF8);
        }

        public static string SerializeToStringStatic<T>(T value)
        {
            return JsonConvert.SerializeObject(value, SerializerSettings);
        }

        public static T DeserializeStatic<T>(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            if (bytes.Length.Equals(0))
                throw new ArgumentException(nameof(bytes));

            var value = bytes.GetString(Encoding.UTF8);
            return DeserializeFromStringStatic<T>(value);
        }

        public static T DeserializeFromStringStatic<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, SerializerSettings);
        }

        public static object DeserializeFromStringStatic(string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type, SerializerSettings);
        }

        public static T CloneStatic<T>(T value)
        {
            if (IsEnumerable<T>())
                return CloneInternal(CreateHelper(value)).Result;

            return CloneInternal(value);
        }
        #endregion

        #region ISerializer Methods
        public override byte[] Serialize<T>(T value)
        {
            return SerializeStatic(value);
        }

        public override string SerializeToString<T>(T value)
        {
            return SerializeToStringStatic(value);
        }

        public override T Deserialize<T>(byte[] bytes)
        {
            return DeserializeStatic<T>(bytes);
        }

        public override T DeserializeFromString<T>(string value)
        {
            return DeserializeFromStringStatic<T>(value);
        }

        public override T Clone<T>(T value)
        {
            return CloneStatic(value);
        }
        #endregion

        #region Private Methods
        private static J CloneInternal<J>(J value)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new JsonSerializer();
                var writer = new BsonWriter(stream);

                serializer.Serialize(writer, value);
                stream.Position = 0;

                var reader = new BsonReader(stream);
                var copy = serializer.Deserialize<J>(reader);

                return copy;
            }
        }

        private static bool IsEnumerable<T>()
        {
            var typeValue = typeof(T);
            return typeValue.GetEnumerableType() != null;
        }

        private static CollectionSerializationHelper<T> CreateHelper<T>(T value)
        {
            var valueType = value.GetType();
            var helperType = typeof(CollectionSerializationHelper<>).MakeGenericType(valueType);

            return (CollectionSerializationHelper<T>)Activator.CreateInstance(helperType, new object[] { value });
        }
        #endregion

        #region Private Classes
        private class CollectionSerializationHelper<T>
        {
            public CollectionSerializationHelper(T result)
            {
                Result = result;
            }

            [JsonProperty]
            public T Result { get; set; }
        }
        #endregion
    }
}

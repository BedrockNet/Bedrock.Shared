using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Bedrock.Shared.Serialization.Binary.Interface;
using Bedrock.Shared.Serialization.Enumeration;

namespace Bedrock.Shared.Serialization.Binary
{
    public class SerializerBinary : SerializerBase, ISerializerBinary
    {
        #region ISerializer Properties
        public override Serializer SerializerType { get { return Serializer.Binary; } }
        #endregion

        #region Public Static Methods
        public static byte[] SerializeStatic<T>(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            byte[] serializedObject;

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();

                formatter.Serialize(stream, value);
                serializedObject = stream.ToArray();
            }

            return serializedObject;
        }

        public static string SerializeToStringStatic<T>(T value)
        {
            var byteArray = SerializeStatic(value);
            return Convert.ToBase64String(byteArray);
        }

        public static T DeserializeStatic<T>(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            if (bytes.Length.Equals(0))
                throw new ArgumentException(nameof(bytes));

            T deserializedObject;

            using (var memoryStream = new MemoryStream(bytes))
            {
                var deserializer = new BinaryFormatter();
                deserializedObject = (T)deserializer.Deserialize(memoryStream);
            }

            return deserializedObject;
        }

        public static T DeserializeFromStringStatic<T>(string value)
        {
            var byteArray = Convert.FromBase64String(value);
            return DeserializeStatic<T>(byteArray);
        }

        public static T CloneStatic<T>(T value)
        {
            var serializedObject = SerializeStatic(value);
            return DeserializeStatic<T>(serializedObject);
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
    }
}

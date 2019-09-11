using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using Bedrock.Shared.Serialization.Enumeration;
using Bedrock.Shared.Serialization.Xml.Interface;

namespace Bedrock.Shared.Serialization.Xml
{
    public class SerializerXml : SerializerBase, ISerializerXml
    {
        #region ISerializer Properties
        public override Serializer SerializerType { get { return Serializer.Xml; } }
        #endregion

        #region Public Static Methods
        public static byte[] SerializeStatic<T>(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            byte[] serializedObject;

            using (var stream = new MemoryStream())
            {
                var serializer = new XmlSerializer(value.GetType());

                serializer.Serialize(stream, value);
                serializedObject = stream.ToArray();
            }

            return serializedObject;
        }

        public static string SerializeToStringStatic<T>(T value)
        {
            var builder = new StringBuilder();
            var serializer = new XmlSerializer(typeof(T));

            using (var writer = new StringWriter(builder, CultureInfo.InvariantCulture))
                serializer.Serialize(writer, value);

            return builder.ToString();
        }

        public static T DeserializeStatic<T>(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            if (bytes.Length.Equals(0))
                throw new ArgumentException(nameof(bytes));

            T deserializedObject;

            using (var stream = new MemoryStream(bytes))
            {
                var serializer = new XmlSerializer(typeof(T));
                deserializedObject = (T)serializer.Deserialize(stream);
            }

            return deserializedObject;
        }

        public static T DeserializeFromStringStatic<T>(string value)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(value))
                return (T)serializer.Deserialize(reader);
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

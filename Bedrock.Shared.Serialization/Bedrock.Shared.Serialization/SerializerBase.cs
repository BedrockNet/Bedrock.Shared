using System;

using Bedrock.Shared.Serialization.Enumeration;
using Bedrock.Shared.Serialization.Interface;

namespace Bedrock.Shared.Serialization
{
    public abstract class SerializerBase : ISerializer
    {
        #region Properties
        public abstract Serializer SerializerType { get; }
        #endregion

        #region ISerializer Methods
        public virtual byte[] Serialize<T>(T value)
        {
            throw new NotImplementedException();
        }

        public virtual byte[] Serialize<T>(T value, Encoding encoding)
        {
            throw new NotImplementedException();
        }

        public virtual string SerializeToString<T>(T value)
        {
            throw new NotImplementedException();
        }

        public virtual string SerializeToString<T>(T value, Encoding encoding)
        {
            throw new NotImplementedException();
        }

        public virtual T Deserialize<T>(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public virtual T Deserialize<T>(byte[] bytes, Encoding encoding)
        {
            throw new NotImplementedException();
        }

        public virtual T DeserializeFromString<T>(string value)
        {
            throw new NotImplementedException();
        }

        public virtual T DeserializeFromString<T>(string value, Encoding encoding)
        {
            throw new NotImplementedException();
        }

        public virtual T Clone<T>(T value)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

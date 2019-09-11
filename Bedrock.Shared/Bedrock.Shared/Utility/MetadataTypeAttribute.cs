using System;

namespace Bedrock.Shared.Utility
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class MetadataTypeAttribute : Attribute
    {
        #region Fields
        private Type _metadataClassType;
        #endregion

        #region Cpnstructors
        public MetadataTypeAttribute(Type metadataClassType)
        {
            _metadataClassType = metadataClassType;
        }
        #endregion

        #region Public Properties
        public Type MetadataClassType
        {
            get
            {
                if (_metadataClassType == null)
                    throw new InvalidOperationException($"{nameof(_metadataClassType)} cannot be null");

                return _metadataClassType;
            }
        }
        #endregion
    }
}

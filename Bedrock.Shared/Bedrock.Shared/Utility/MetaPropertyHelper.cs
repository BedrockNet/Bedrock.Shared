using System;
using System.Linq;
using System.Reflection;

namespace Bedrock.Shared.Utility
{
    public static class MetaPropertyHelper
    {
        #region Public Methods
        public static PropertyInfo[] GetMetaProperties(Type type, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
        {
            var meta = ApplicationContext.DomainGraphCache.GetAttributeOfType<MetadataTypeAttribute>(type);

            if (meta == null)
                return null;

            return ApplicationContext.DomainGraphCache.GetProperties(meta.MetadataClassType, flags).Values.ToArray();
        }

        public static PropertyInfo GetMetaProperty(PropertyInfo[] metaProperties, PropertyInfo property)
        {
            if (metaProperties == null)
                return null;

            return metaProperties.Where(mp => mp.Name == property.Name).SingleOrDefault();
        }

        public static PropertyInfo GetProperty(PropertyInfo[] metaProperties, PropertyInfo property)
        {
            var metaProperty = GetMetaProperty(metaProperties, property);

            if (metaProperty == null)
                return property;

            return metaProperty;
        }
        #endregion
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bedrock.Shared.Utility
{
    public class DomainGraphCache
    {
        #region Fields
        private ConcurrentDictionary<Tuple<Type, BindingFlags>, Dictionary<string, PropertyInfo>> _typeProperties;
        private ConcurrentDictionary<Type, Dictionary<Type, object>> _typeAttributes;
        private ConcurrentDictionary<Tuple<Tuple<Type, BindingFlags>, Type>, object> _typePropertyAttributes;
        #endregion

        #region Constructor
        public DomainGraphCache()
        {
            _typeProperties = new ConcurrentDictionary<Tuple<Type, BindingFlags>, Dictionary<string, PropertyInfo>>();
            _typeAttributes = new ConcurrentDictionary<Type, Dictionary<Type, object>>();
            _typePropertyAttributes = new ConcurrentDictionary<Tuple<Tuple<Type, BindingFlags>, Type>, object>();
        }
        #endregion

        #region Public Static Methods
        public static Dictionary<string, PropertyInfo> GetFilteredProperties(Type t, BindingFlags flags)
        {
            var result = new Dictionary<string, PropertyInfo>();

            while (t != typeof(object))
            {
                foreach (var prop in t.GetTypeInfo().GetProperties(BindingFlags.DeclaredOnly | flags))
                {
                    if (!result.Keys.Contains(prop.Name))
                        result.Add(prop.Name, prop);
                }

                t = t.GetTypeInfo().BaseType;
            }

            return result;
        }
        #endregion

        #region Public Methods
        public IEnumerable<PropertyInfo> GetPropertiesOfType<T>(Type parent, BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            return GetPropertiesOfType<T>(parent, false, flags);
        }

        public IEnumerable<PropertyInfo> GetPropertiesOfType<T>(Type parent, bool recursive, BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            if (recursive)
                return GetAllChildPropertiesOfType<T>(parent, flags);
            else
                return GetProperties(parent, flags).Values.Where(y => typeof(T).GetTypeInfo().IsAssignableFrom(y.PropertyType));
        }

        public IEnumerable<Type> GetGenericArgumentTypesOfType<T>(Type parent, BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            var types = GetProperties(parent, flags).Values;

            return types.Where(x => x.PropertyType.GetTypeInfo().ContainsGenericParameters).SelectMany(x => x.PropertyType.GetTypeInfo().GetGenericArguments().Where(y => typeof(T).GetTypeInfo().IsAssignableFrom(y)));
        }

        public T GetAttributeOfType<T>(Type parent)
        {
            return (T)GetAttributes(parent).Values.Where(y => typeof(T).GetTypeInfo().IsAssignableFrom(y.GetType())).FirstOrDefault();
        }

        public Dictionary<string, PropertyInfo> GetProperties(Type parent, BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            return _typeProperties.GetOrAdd(new Tuple<Type, BindingFlags>(parent, flags), x => GetPropertyDictionary(parent, flags));
        }

        public Dictionary<Type, object> GetAttributes(Type parent)
        {
            var attributes = parent.GetTypeInfo().GetCustomAttributes(false).ToDictionary(k => k.GetType(), e => (object)e);
            return _typeAttributes.GetOrAdd(parent, attributes);
        }

        public PropertyInfo GetProperty(Type parent, string propertyName, BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            return GetProperties(parent, flags)[propertyName];
        }

        public object GetAttribute(Type parent, Type attribute)
        {
            return GetAttributes(parent)[attribute];
        }

        public Dictionary<PropertyInfo, T> GetPropertiesAttributes<T>(Type parent, BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public) where T : Attribute
        {
            return _typePropertyAttributes.GetOrAdd(new Tuple<Tuple<Type, BindingFlags>, Type>(new Tuple<Type, BindingFlags>(parent, flags), typeof(T)), combo =>
            {
                var typePropAttributes = new Dictionary<PropertyInfo, T>();
                var props = GetProperties(parent, flags).Values;

                foreach (var prop in props)
                {
                    var t = prop.GetCustomAttributes(false).Where(ca => ca is T).SingleOrDefault() as T;

                    if (t != null)
                        typePropAttributes.Add(prop, t);
                }

                return typePropAttributes;
            }) as Dictionary<PropertyInfo, T>;
        }
        #endregion

        #region Private Static Methods
        private static Dictionary<string, PropertyInfo> GetPropertyDictionary(Type parent, BindingFlags flags)
        {
            var result = parent.GetTypeInfo().GetProperties(flags);
            var names = result.Select(x => x.Name);

            if (names.Count() != names.Distinct().Count())
                return GetFilteredProperties(parent, flags);

            return result.ToDictionary(x => x.Name);
        }
        #endregion

        #region Private Methods
        private List<PropertyInfo> GetAllChildPropertiesOfType<T>(Type type, BindingFlags flags, List<Type> types = null, List<PropertyInfo> properties = null)
        {
            if (properties == null)
                properties = new List<PropertyInfo>();

            if (types == null)
                types = new List<Type>();

            var typeProperties = GetProperties(type, flags).Values.Where(y => typeof(T).GetTypeInfo().IsAssignableFrom(y.PropertyType) && !properties.Contains(y)).ToList();
            properties.AddRange(typeProperties);
            types.Add(type);

            foreach (var property in typeProperties)
            {
                if (!types.Contains(property.PropertyType))
                    GetAllChildPropertiesOfType<T>(property.PropertyType, flags, types, properties);
            }

            var genericTypes = GetProperties(type, flags).Values.Where(y => y.PropertyType.GetTypeInfo().IsGenericType).SelectMany(y => y.PropertyType.GetTypeInfo().GetGenericArguments()).Where(y => typeof(T).GetTypeInfo().IsAssignableFrom(y) && !types.Contains(y)).ToList();

            foreach (var genericType in genericTypes)
                GetAllChildPropertiesOfType<T>(genericType, flags, types, properties);

            return properties;
        }
        #endregion
    }
}

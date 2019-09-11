using System;
using System.Collections.Generic;
using System.Linq;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Utility;

namespace Bedrock.Shared.Extension
{
    public static class TypeExtension
    {
        #region Public Methods
        public static Type GetDomainType(this Type type)
        {
            var isProxy = type.Namespace == StringHelper.Current.Lookup(StringApplication.EntityFrameworkProxyNamespace);
            return isProxy ? type.BaseType : type;
        }

        public static Type GetEnumerableType(this Type type)
        {
            foreach (var t in type.GetInterfaces())
            {
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    return t.GetGenericArguments()[0];
            }

            return null;
        }

        public static bool IsPrimitive(this Type type)
        {
            return type.IsValueType || type.IsPrimitive || new Type[]
            {
                    typeof(string),
                    typeof(decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
            }.Contains(type) || Convert.GetTypeCode(type) != TypeCode.Object;
        }
        #endregion
    }
}

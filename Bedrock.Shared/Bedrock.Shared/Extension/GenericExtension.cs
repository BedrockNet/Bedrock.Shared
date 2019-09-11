using System.Collections.Generic;
using System.Reflection;

namespace Bedrock.Shared.Extension
{
    public static class GenericExtension
    {
        #region Public Methods
        public static bool PublicInstancePropertiesEqual<T>(this T self, T to, params string[] ignore) where T : class
        {
            if (self != null && to != null)
            {
                var type = typeof(T);
                var ignoreList = new List<string>(ignore);

                foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    if (!ignoreList.Contains(property.Name))
                    {
                        var selfValue = type.GetProperty(property.Name).GetValue(self, null);
                        var toValue = type.GetProperty(property.Name).GetValue(to, null);

                        if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                            return false;
                    }
                }

                return true;
            }

            return self == to;
        }

        public static T DefaultIfNull<T>(this T? value, T replacementValue) where T : struct
        {
            return value.HasValue ? value.Value : replacementValue;
        }
        #endregion
    }
}

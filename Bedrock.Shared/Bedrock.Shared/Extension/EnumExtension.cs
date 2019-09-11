using System;
using System.Linq;

using Bedrock.Shared.Utility;

namespace Bedrock.Shared.Extension
{
    public static class EnumExtension
    {
        #region Public Methods
        public static string BedrockFieldValue(this Enum enumValue)
        {
            var attributes = (BedrockFieldValueAttribute[])enumValue.GetType().GetField(enumValue.ToString()).GetCustomAttributes(typeof(BedrockFieldValueAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Value;

            throw new InvalidOperationException("No Bedrock Value Attribute defined for " + enumValue);
        }

        public static T ToEnumFromBedrockFieldValue<T>(this string itemValue)
        {
            var type = typeof(T);

            if (!type.IsEnum)
                throw new InvalidOperationException();

            foreach (var field in from field in type.GetFields()
                                  let attributes = (BedrockFieldValueAttribute[])field.GetCustomAttributes(typeof(BedrockFieldValueAttribute), false)
                                  where attributes.Length > 0 && attributes[0].Value == itemValue
                                  select field)
            {
                return (T)field.GetValue(null);
            }

            throw new InvalidOperationException("No Bedrock Field Value found for " + itemValue);
        }
        #endregion
    }
}

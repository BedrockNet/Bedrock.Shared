using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

using Bedrock.Shared.Utility;
using Bedrock.Shared.Data.Validation.Interface;

using SharedUtility = Bedrock.Shared.Utility;

namespace Bedrock.Shared.Data.Validation.Implementation.Field
{
    public class DataAnnotationsValidationRuleProvider : IValidationRuleProvider
    {
        #region IValidationRuleProvider
        public IEnumerable<IValidationRule> GetValidationRules(Type type)
        {
            var rules = new List<IValidationRule>();
            var properties = SharedUtility.ApplicationContext.DomainGraphCache.GetProperties(type, BindingFlags.Instance | BindingFlags.Public).Values.ToArray();
            var metaProperties = MetaPropertyHelper.GetMetaProperties(type);

            foreach (var property in properties)
            {
                var propertyInfo = MetaPropertyHelper.GetProperty(metaProperties, property);
                var validators = propertyInfo.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>();

                foreach (var validator in validators)
                    rules.Add(new DataAnnotationsValidationRule(new PropertyDescriptor(property), validator));
            }

            return rules;
        }
        #endregion
    }
}

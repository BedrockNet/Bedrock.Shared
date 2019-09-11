using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Extension;
using Bedrock.Shared.Utility;
using Bedrock.Shared.Data.Validation.Implementation.Field;
using Bedrock.Shared.Data.Validation.Interface;

namespace Bedrock.Shared.Data.Validation.Implementation.Configuration
{
    public class ValidationConfigurationRules : IValidationConfigurationRules
    {
        #region Constructors
        public ValidationConfigurationRules()
        {
            EntityRules = new ConcurrentBag<IValidationRule>();
            EntityRuleExpressions = new ConcurrentBag<IValidationRuleExpression>();
        }
        #endregion

        #region Public Properties
        public ConcurrentBag<IValidationRule> EntityRules { get; private set; }

        public ConcurrentBag<IValidationRuleExpression> EntityRuleExpressions { get; private set; }
        #endregion
    }

    public class ValidationConfigurationRules<TEntity> : ValidationConfigurationRules, IValidationConfigurationRules<TEntity> where TEntity : class
    {
        #region Public Methods (AddRules)
        public IValidationConfigurationRules<TEntity> AddRule(params ValidationAttribute[] validationAttributes)
        {
            validationAttributes.Each(va => EntityRules.Add(new DataAnnotationsValidationRuleClass(va)));
            return this;
        }

        public IValidationConfigurationRules<TEntity> AddRule<TProperty>(Expression<Func<TEntity, TProperty>> property, params ValidationAttribute[] validationAttributes)
        {
            var memberExpression = property.Body as MemberExpression;
            var propertyInfo = memberExpression.Member as PropertyInfo;

            validationAttributes.Each(va => EntityRules.Add(new DataAnnotationsValidationRule(new PropertyDescriptor(propertyInfo), va)));

            return this;
        }

        public IValidationConfigurationRules<TEntity> AddRuleCreditCard<TProperty>(Expression<Func<TEntity, TProperty>> property, string errorMessage = null)
        {
            var attribute = AttributeCreator.CreateCreditCardAttribute(errorMessage);
            return AddRule(property, attribute);
        }

        public IValidationConfigurationRules<TEntity> AddRuleCustom(Type validatorType, string method, string errorMessage = null)
        {
            var attribute = AttributeCreator.CreateCustomAttribute(validatorType, method, errorMessage);
            return AddRule(attribute);
        }

        public IValidationConfigurationRules<TEntity> AddRuleCustom<TProperty>(Expression<Func<TEntity, TProperty>> property, Type validatorType, string method, string errorMessage = null)
        {
            var attribute = AttributeCreator.CreateCustomAttribute(validatorType, method, errorMessage);
            return AddRule(property, attribute);
        }

        public IValidationConfigurationRules<TEntity> AddRuleEmailAddress<TProperty>(Expression<Func<TEntity, TProperty>> property, string errorMessage = null)
        {
            var attribute = AttributeCreator.CreateEmailAddressAttribute(errorMessage);
            return AddRule(property, attribute);
        }

        public IValidationConfigurationRules<TEntity> AddRuleMinLength<TProperty>(Expression<Func<TEntity, TProperty>> property, int length, string errorMessage = null)
        {
            var attribute = AttributeCreator.CreateMinLengthAttribute(length, errorMessage);
            return AddRule(property, attribute);
        }

        public IValidationConfigurationRules<TEntity> AddRuleMaxLength<TProperty>(Expression<Func<TEntity, TProperty>> property, int length, string errorMessage = null)
        {
            var attribute = AttributeCreator.CreateMaxLengthAttribute(length, errorMessage);
            return AddRule(property, attribute);
        }

        public IValidationConfigurationRules<TEntity> AddRulePhone<TProperty>(Expression<Func<TEntity, TProperty>> property, string errorMessage = null)
        {
            var attribute = AttributeCreator.CreatePhoneAttribute(errorMessage);
            return AddRule(property, attribute);
        }

        public IValidationConfigurationRules<TEntity> AddRuleRange<TProperty>(Expression<Func<TEntity, TProperty>> property, double minimum, double maximum, string errorMessage = null)
        {
            var attribute = AttributeCreator.CreateRangeAttribute(minimum, maximum, errorMessage);
            return AddRule(property, attribute);
        }

        public IValidationConfigurationRules<TEntity> AddRuleRange<TProperty>(Expression<Func<TEntity, TProperty>> property, int minimum, int maximum, string errorMessage = null)
        {
            var attribute = AttributeCreator.CreateRangeAttribute(minimum, maximum, errorMessage);
            return AddRule(property, attribute);
        }

        public IValidationConfigurationRules<TEntity> AddRuleRange<TProperty>(Expression<Func<TEntity, TProperty>> property, Type typeToTest, string minimum, string maximum, string errorMessage = null)
        {
            var attribute = AttributeCreator.CreateRangeAttribute(typeToTest, minimum, maximum, errorMessage);
            return AddRule(property, attribute);
        }

        public IValidationConfigurationRules<TEntity> AddRuleRegularExpression<TProperty>(Expression<Func<TEntity, TProperty>> property, string pattern, string errorMessage = null)
        {
            var attribute = AttributeCreator.CreateRegularExpressionAttribute(pattern, errorMessage);
            return AddRule(property, attribute);
        }

        public IValidationConfigurationRules<TEntity> AddRuleRequired<TProperty>(Expression<Func<TEntity, TProperty>> property, string errorMessage = null)
        {
            var attribute = AttributeCreator.CreateRequiredAttribute(errorMessage);
            return AddRule(property, attribute);
        }

        public IValidationConfigurationRules<TEntity> AddRuleRequired<TProperty>(Expression<Func<TEntity, TProperty>> property, bool? allowEmptyStrings = null, string errorMessage = null)
        {
            var attribute = AttributeCreator.CreateRequiredAttribute(allowEmptyStrings, errorMessage);
            return AddRule(property, attribute);
        }

        public IValidationConfigurationRules<TEntity> AddRuleStringLength<TProperty>(Expression<Func<TEntity, TProperty>> property, int maximumLength, string errorMessage = null)
        {
            var attribute = AttributeCreator.CreateStringLengthAttribute(maximumLength, errorMessage);
            return AddRule(property, attribute);
        }

        public IValidationConfigurationRules<TEntity> AddRuleStringLength<TProperty>(Expression<Func<TEntity, TProperty>> property, int maximumLength, int? minimumLength = null, string errorMessage = null)
        {
            var attribute = AttributeCreator.CreateStringLengthAttribute(maximumLength, minimumLength, errorMessage);
            return AddRule(property, attribute);
        }
        #endregion

        #region Public Methods (AddExpression)
        public IValidationConfigurationRules<TEntity> AddExpression(Expression<Func<TEntity, bool>> ruleExpression, string errorMessage)
        {
            errorMessage = !string.IsNullOrWhiteSpace(errorMessage) ? errorMessage : StringHelper.Current.Lookup(StringError.RuleEvaluationFailed);
            EntityRuleExpressions.Add(new ValidationRuleExpression<TEntity>(ruleExpression, errorMessage));
            return this;
        }
        #endregion
    }
}

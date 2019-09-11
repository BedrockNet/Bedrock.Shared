using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Bedrock.Shared.Data.Validation.Interface
{
    public interface IValidationConfigurationRules
    {
        #region Properties
        ConcurrentBag<IValidationRule> EntityRules { get; }

        ConcurrentBag<IValidationRuleExpression> EntityRuleExpressions { get; }
        #endregion
    }

    public interface IValidationConfigurationRules<TEntity> : IValidationConfigurationRules where TEntity : class
    {
        #region Methods
        IValidationConfigurationRules<TEntity> AddRule(params ValidationAttribute[] validationAttributes);

        IValidationConfigurationRules<TEntity> AddRule<TProperty>(Expression<Func<TEntity, TProperty>> property, params ValidationAttribute[] validationAttributes);

        IValidationConfigurationRules<TEntity> AddRuleCreditCard<TProperty>(Expression<Func<TEntity, TProperty>> property, string errorMessage = null);

        IValidationConfigurationRules<TEntity> AddRuleCustom(Type validatorType, string method, string errorMessage = null);

        IValidationConfigurationRules<TEntity> AddRuleCustom<TProperty>(Expression<Func<TEntity, TProperty>> property, Type validatorType, string method, string errorMessage = null);

        IValidationConfigurationRules<TEntity> AddRuleEmailAddress<TProperty>(Expression<Func<TEntity, TProperty>> property, string errorMessage = null);

        IValidationConfigurationRules<TEntity> AddRuleMinLength<TProperty>(Expression<Func<TEntity, TProperty>> property, int length, string errorMessage = null);

        IValidationConfigurationRules<TEntity> AddRuleMaxLength<TProperty>(Expression<Func<TEntity, TProperty>> property, int length, string errorMessage = null);

        IValidationConfigurationRules<TEntity> AddRulePhone<TProperty>(Expression<Func<TEntity, TProperty>> property, string errorMessage = null);

        IValidationConfigurationRules<TEntity> AddRuleRange<TProperty>(Expression<Func<TEntity, TProperty>> property, double minimum, double maximum, string errorMessage = null);

        IValidationConfigurationRules<TEntity> AddRuleRange<TProperty>(Expression<Func<TEntity, TProperty>> property, int minimum, int maximum, string errorMessage = null);

        IValidationConfigurationRules<TEntity> AddRuleRange<TProperty>(Expression<Func<TEntity, TProperty>> property, Type typeToTest, string minimum, string maximum, string errorMessage = null);

        IValidationConfigurationRules<TEntity> AddRuleRegularExpression<TProperty>(Expression<Func<TEntity, TProperty>> property, string pattern, string errorMessage = null);

        IValidationConfigurationRules<TEntity> AddRuleRequired<TProperty>(Expression<Func<TEntity, TProperty>> property, string errorMessage = null);

        IValidationConfigurationRules<TEntity> AddRuleRequired<TProperty>(Expression<Func<TEntity, TProperty>> property, bool? allowEmptyStrings = null, string errorMessage = null);

        IValidationConfigurationRules<TEntity> AddRuleStringLength<TProperty>(Expression<Func<TEntity, TProperty>> property, int maximumLength, string errorMessage = null);

        IValidationConfigurationRules<TEntity> AddRuleStringLength<TProperty>(Expression<Func<TEntity, TProperty>> property, int maximumLength, int? minimumLength = null, string errorMessage = null);

        IValidationConfigurationRules<TEntity> AddExpression(Expression<Func<TEntity, bool>> ruleExpression, string errorMessage);
        #endregion
    }
}

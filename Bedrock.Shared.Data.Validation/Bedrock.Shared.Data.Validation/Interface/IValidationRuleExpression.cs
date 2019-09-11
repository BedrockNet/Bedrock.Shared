using System;
using System.Linq.Expressions;

namespace Bedrock.Shared.Data.Validation.Interface
{
    public interface IValidationRuleExpression
    {
        #region Properties
        string ErrorMessage { get; set; }
        #endregion

        #region Methods
        bool Evaluate(object target);
        #endregion
    }

    public interface IValidationRuleExpression<TEntity> : IValidationRuleExpression where TEntity : class
    {
        #region Properties
        Expression<Func<TEntity, bool>> RuleExpression { get; set; }
        #endregion
    }
}

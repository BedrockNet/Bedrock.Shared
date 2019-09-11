using System;
using System.Linq.Expressions;

using Bedrock.Shared.Data.Validation.Interface;

namespace Bedrock.Shared.Data.Validation.Implementation
{
    public abstract class ValidationRuleExpression : IValidationRuleExpression
    {
        #region IValidationExpression Properties
        public string ErrorMessage { get; set; }

        public abstract bool Evaluate(object target);
        #endregion
    }

    public class ValidationRuleExpression<TEntity> : ValidationRuleExpression, IValidationRuleExpression<TEntity> where TEntity : class
    {
        #region Constructors
        public ValidationRuleExpression(Expression<Func<TEntity, bool>> ruleExpression, string errrorMessage)
        {
            RuleExpression = ruleExpression;
            ErrorMessage = errrorMessage;
        }
        #endregion

        #region IValidationRuleExpression<TEntity> Properties
        public Expression<Func<TEntity, bool>> RuleExpression { get; set; }
        #endregion

        #region IValidationRuleExpression Methods
        public override bool Evaluate(object target)
        {
            return EvaluateInternal((TEntity)target);
        }
        #endregion

        #region Private Methods
        private bool EvaluateInternal(TEntity target)
        {
            var func = RuleExpression.Compile();
            return func.Invoke(target);
        }
        #endregion
    }
}

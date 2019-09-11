using System.Linq.Expressions;

namespace Bedrock.Shared.Utility
{
    public class ReplaceExpressionVisitor : ExpressionVisitor
    {
        #region Fields
        private readonly Expression _oldValue;
        private readonly Expression _newValue;
        #endregion

        #region Constructors
        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }
        #endregion

        #region ExpressionVisitor Members
        public override Expression Visit(Expression node)
        {
            if (node == _oldValue)
                return _newValue;

            return base.Visit(node);
        }
        #endregion
    }
}

using Bedrock.Shared.Data.Validation.Interface;

namespace Bedrock.Shared.Data.Validation.Implementation.Field
{
    public abstract class ValidationRule : IValidationRule
    {
        #region Fields
        private IPropertyDescriptor _property;
        #endregion

        #region Constructor
        protected ValidationRule() { }

        protected ValidationRule(IPropertyDescriptor property)
        {
            _property = property;
        }
        #endregion

        #region Properties
        public IPropertyDescriptor Property
        {
            get { return _property; }
        }
        #endregion

        #region Methods
        public abstract IValidationResult Execute(object target);
        #endregion
    }
}

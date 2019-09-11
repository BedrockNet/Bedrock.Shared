using Bedrock.Shared.Data.Validation.Enumeration;
using Bedrock.Shared.Data.Validation.Interface;

using DataAnnotations = System.ComponentModel.DataAnnotations;

namespace Bedrock.Shared.Data.Validation.Implementation.Field
{
    public class DataAnnotationsValidationRuleClass : ValidationRule
    {
        #region Fields
        private DataAnnotations.ValidationAttribute _validationAttribute;
        #endregion

        #region Constructor
        public DataAnnotationsValidationRuleClass(DataAnnotations.ValidationAttribute validationAttribute)
        {
            _validationAttribute = validationAttribute;
        }
        #endregion

        #region Methods
        public override IValidationResult Execute(object target)
        {
            var result = _validationAttribute.GetValidationResult(target, CreateContext(target));

            if (result != DataAnnotations.ValidationResult.Success)
                return new ValidationResult(Property, result.ErrorMessage, ValidationResultType.Field);
            else
                return ValidationResult.ValidField;
        }

        private DataAnnotations.ValidationContext CreateContext(object target)
        {
            var context = ApplicationContext.GetValidationContext(target);
            context.MemberName = target.GetType().Name;
            return context;
        }
        #endregion
    }
}

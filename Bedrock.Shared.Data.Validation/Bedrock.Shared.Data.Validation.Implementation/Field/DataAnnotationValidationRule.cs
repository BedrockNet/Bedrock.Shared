using Bedrock.Shared.Data.Validation.Enumeration;
using Bedrock.Shared.Data.Validation.Interface;

using DataAnnotations = System.ComponentModel.DataAnnotations;

namespace Bedrock.Shared.Data.Validation.Implementation.Field
{
    public class DataAnnotationsValidationRule : ValidationRule
    {
        #region Fields
        private DataAnnotations.ValidationAttribute _validationAttribute;
        #endregion

        #region Constructor
        public DataAnnotationsValidationRule(IPropertyDescriptor property, DataAnnotations.ValidationAttribute validationAttribute) : base(property)
        {
            _validationAttribute = validationAttribute;
        }
        #endregion

        #region Methods
        public override IValidationResult Execute(object target)
        {
            var result = _validationAttribute.GetValidationResult(Property.GetValue(target), CreateContext(target));

            if (result != DataAnnotations.ValidationResult.Success)
                return new ValidationResult(Property, result.ErrorMessage, ValidationResultType.Field);
            else
                return ValidationResult.ValidField;
        }

        private DataAnnotations.ValidationContext CreateContext(object target)
        {
            var context = ApplicationContext.GetValidationContext(target);
            context.MemberName = Property.Name;
            return context;
        }
        #endregion
    }
}

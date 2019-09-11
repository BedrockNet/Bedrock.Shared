using System;
using System.ComponentModel.DataAnnotations;

namespace Bedrock.Shared.Data.Validation.Implementation.Configuration
{
    public static class AttributeCreator
    {
        #region Public Methods
        public static CreditCardAttribute CreateCreditCardAttribute(string errorMessage = null)
        {
            var returnValue = new CreditCardAttribute();
            UpdateErrorMessage(returnValue, errorMessage);
            return returnValue;
        }

        public static CustomValidationAttribute CreateCustomAttribute(Type validatorType, string method, string errorMessage = null)
        {
            var returnValue = new CustomValidationAttribute(validatorType, method);
            UpdateErrorMessage(returnValue, errorMessage);
            return returnValue;
        }

        public static EmailAddressAttribute CreateEmailAddressAttribute(string errorMessage = null)
        {
            var returnValue = new EmailAddressAttribute();
            UpdateErrorMessage(returnValue, errorMessage);
            return returnValue;
        }

        public static MinLengthAttribute CreateMinLengthAttribute(int length, string errorMessage = null)
        {
            var returnValue = new MinLengthAttribute(length);
            UpdateErrorMessage(returnValue, errorMessage);
            return returnValue;
        }

        public static MaxLengthAttribute CreateMaxLengthAttribute(int length, string errorMessage = null)
        {
            var returnValue = new MaxLengthAttribute(length);
            UpdateErrorMessage(returnValue, errorMessage);
            return returnValue;
        }

        public static PhoneAttribute CreatePhoneAttribute(string errorMessage = null)
        {
            var returnValue = new PhoneAttribute();
            UpdateErrorMessage(returnValue, errorMessage);
            return returnValue;
        }

        public static RangeAttribute CreateRangeAttribute(double minimum, double maximum, string errorMessage = null)
        {
            var returnValue = new RangeAttribute(minimum, maximum);
            UpdateErrorMessage(returnValue, errorMessage);
            return returnValue;
        }

        public static RangeAttribute CreateRangeAttribute(int minimum, int maximum, string errorMessage = null)
        {
            var returnValue = new RangeAttribute(minimum, maximum);
            UpdateErrorMessage(returnValue, errorMessage);
            return returnValue;
        }

        public static RangeAttribute CreateRangeAttribute(Type typeToTest, string minimum, string maximum, string errorMessage = null)
        {
            var returnValue = new RangeAttribute(typeToTest, minimum, maximum);
            UpdateErrorMessage(returnValue, errorMessage);
            return returnValue;
        }

        public static RegularExpressionAttribute CreateRegularExpressionAttribute(string pattern, string errorMessage)
        {
            var returnValue = new RegularExpressionAttribute(pattern);
            UpdateErrorMessage(returnValue, errorMessage);
            return returnValue;
        }

        public static RequiredAttribute CreateRequiredAttribute(string errorMessage)
        {
            return CreateRequiredAttribute(null, errorMessage);
        }

        public static RequiredAttribute CreateRequiredAttribute(bool? allowEmptyStrings = null, string errorMessage = null)
        {
            var returnValue = new RequiredAttribute();

            if(allowEmptyStrings.HasValue)
                returnValue.AllowEmptyStrings = allowEmptyStrings.Value;

            UpdateErrorMessage(returnValue, errorMessage);

            return returnValue;
        }

        public static StringLengthAttribute CreateStringLengthAttribute(int maximumLength, string errorMessage)
        {
            return CreateStringLengthAttribute(maximumLength, null, errorMessage);
        }

        public static StringLengthAttribute CreateStringLengthAttribute(int maximumLength, int? minimumLength = null, string errorMessage = null)
        {
            var returnValue = new StringLengthAttribute(maximumLength);

            if (minimumLength.HasValue)
                returnValue.MinimumLength = minimumLength.Value;

            UpdateErrorMessage(returnValue, errorMessage);
            return returnValue;
        }
        #endregion

        #region Private Methods
        private static void UpdateErrorMessage(ValidationAttribute attribute, string errorMessage)
        {
            if (!string.IsNullOrEmpty(errorMessage))
                attribute.ErrorMessage = errorMessage;
        }
        #endregion
    }
}

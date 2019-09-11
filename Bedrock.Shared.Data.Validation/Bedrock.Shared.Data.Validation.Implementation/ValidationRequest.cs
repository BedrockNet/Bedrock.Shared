using Bedrock.Shared.Data.Validation.Enumeration;
using Bedrock.Shared.Data.Validation.Interface;

namespace Bedrock.Shared.Data.Validation.Implementation
{
    public class ValidationRequest : IValidationRequest
    {
        #region Constructors
        private ValidationRequest() { }
        #endregion

        #region Properties
        public bool IsFields { get; set; }

        public bool IsRules { get; set; }

        public bool IsRecursiveFields { get; set; }

        public bool IsRecursiveCollectionFields { get; set; }

        public bool IsHaltOnErrorFields { get; set; }

        public bool IsRecursiveRules { get; set; }

        public bool IsRecursiveCollectionRules { get; set; }

        public bool IsHaltOnErrorRules { get; set; }

        public bool ThrowExceptionOnError { get; set; }

        public ValidationRequestType RequestType { get; private set; }
        #endregion

        #region Static Methods (ValidationRequest)
        public static IValidationRequest None()
        {
            return new ValidationRequest
            {
                IsFields = false,
                IsRules = false,
                IsRecursiveFields = false,
                IsRecursiveCollectionFields = false,
                IsHaltOnErrorFields = false,
                IsRecursiveRules = false,
                IsRecursiveCollectionRules = false,
                IsHaltOnErrorRules = false,
                ThrowExceptionOnError = false,
                RequestType = ValidationRequestType.None
            };
        }

        public static IValidationRequest Full()
        {
            return Full(false);
        }

        public static IValidationRequest Full(bool throwExceptionOnError)
        {
            return new ValidationRequest
            {
                IsFields = true,
                IsRules = true,
                IsRecursiveFields = true,
                IsRecursiveCollectionFields = true,
                IsHaltOnErrorFields = false,
                IsRecursiveRules = true,
                IsRecursiveCollectionRules = true,
                IsHaltOnErrorRules = false,
                ThrowExceptionOnError = throwExceptionOnError,
                RequestType = ValidationRequestType.Full
            };
        }

        public static IValidationRequest FullHaltOnError()
        {
            return FullHaltOnError(false);
        }

        public static IValidationRequest FullHaltOnError(bool throwExceptionOnError)
        {
            return new ValidationRequest
            {
                IsFields = true,
                IsRules = true,
                IsRecursiveFields = true,
                IsRecursiveCollectionFields = true,
                IsHaltOnErrorFields = true,
                IsRecursiveRules = true,
                IsRecursiveCollectionRules = true,
                IsHaltOnErrorRules = true,
                ThrowExceptionOnError = throwExceptionOnError,
                RequestType = ValidationRequestType.FullHaltOnError
            };
        }

        public static IValidationRequest FullHaltOnErrorFields()
        {
            return FullHaltOnErrorFields(false);
        }

        public static IValidationRequest FullHaltOnErrorFields(bool throwExceptionOnError)
        {
            return new ValidationRequest
            {
                IsFields = true,
                IsRules = true,
                IsRecursiveFields = true,
                IsRecursiveCollectionFields = true,
                IsHaltOnErrorFields = true,
                IsRecursiveRules = true,
                IsRecursiveCollectionRules = true,
                IsHaltOnErrorRules = false,
                ThrowExceptionOnError = throwExceptionOnError,
                RequestType = ValidationRequestType.FullHaltOnErrorFields
            };
        }

        public static IValidationRequest FullHaltOnErrorRules()
        {
            return FullHaltOnErrorRules(false);
        }

        public static IValidationRequest FullHaltOnErrorRules(bool throwExceptionOnError)
        {
            return new ValidationRequest
            {
                IsFields = true,
                IsRules = true,
                IsRecursiveFields = true,
                IsRecursiveCollectionFields = true,
                IsHaltOnErrorFields = false,
                IsRecursiveRules = true,
                IsRecursiveCollectionRules = true,
                IsHaltOnErrorRules = true,
                ThrowExceptionOnError = throwExceptionOnError,
                RequestType = ValidationRequestType.FullHaltOnErrorRules
            };
        }

        public static IValidationRequest FullNoRecursion()
        {
            return FullNoRecursion(false);
        }

        public static IValidationRequest FullNoRecursion(bool throwExceptionOnError)
        {
            return new ValidationRequest
            {
                IsFields = true,
                IsRules = true,
                IsRecursiveFields = false,
                IsRecursiveCollectionFields = false,
                IsHaltOnErrorFields = false,
                IsRecursiveRules = false,
                IsRecursiveCollectionRules = false,
                IsHaltOnErrorRules = false,
                ThrowExceptionOnError = throwExceptionOnError,
                RequestType = ValidationRequestType.FullNoRecursion
            };
        }

        public static IValidationRequest FieldsRecursive()
        {
            return FieldsRecursive(false);
        }

        public static IValidationRequest FieldsRecursive(bool throwExceptionOnError)
        {
            return new ValidationRequest
            {
                IsFields = true,
                IsRules = false,
                IsRecursiveFields = true,
                IsRecursiveCollectionFields = true,
                IsHaltOnErrorFields = false,
                IsRecursiveRules = false,
                IsRecursiveCollectionRules = false,
                IsHaltOnErrorRules = false,
                ThrowExceptionOnError = throwExceptionOnError,
                RequestType = ValidationRequestType.FieldsRecursive
            };
        }

        public static IValidationRequest FieldsHaltOnErrorRecursive()
        {
            return FieldsHaltOnErrorRecursive(false);
        }

        public static IValidationRequest FieldsHaltOnErrorRecursive(bool throwExceptionOnError)
        {
            return new ValidationRequest
            {
                IsFields = true,
                IsRules = false,
                IsRecursiveFields = true,
                IsRecursiveCollectionFields = true,
                IsHaltOnErrorFields = true,
                IsRecursiveRules = false,
                IsRecursiveCollectionRules = false,
                IsHaltOnErrorRules = false,
                ThrowExceptionOnError = throwExceptionOnError,
                RequestType = ValidationRequestType.FieldsHaltOnErrorRecursive
            };
        }

        public static IValidationRequest FieldsNoCollectionRecursive()
        {
            return FieldsNoCollectionRecursive(false);
        }

        public static IValidationRequest FieldsNoCollectionRecursive(bool throwExceptionOnError)
        {
            return new ValidationRequest
            {
                IsFields = true,
                IsRules = false,
                IsRecursiveFields = true,
                IsRecursiveCollectionFields = false,
                IsHaltOnErrorFields = false,
                IsRecursiveRules = false,
                IsRecursiveCollectionRules = false,
                IsHaltOnErrorRules = false,
                ThrowExceptionOnError = throwExceptionOnError,
                RequestType = ValidationRequestType.FieldsNoCollectionRecursive
            };
        }

        public static IValidationRequest FieldsNoCollectionHaltOnErrorRecursive()
        {
            return FieldsNoCollectionHaltOnErrorRecursive(false);
        }

        public static IValidationRequest FieldsNoCollectionHaltOnErrorRecursive(bool throwExceptionOnError)
        {
            return new ValidationRequest
            {
                IsFields = true,
                IsRules = false,
                IsRecursiveFields = true,
                IsRecursiveCollectionFields = false,
                IsHaltOnErrorFields = true,
                IsRecursiveRules = false,
                IsRecursiveCollectionRules = false,
                IsHaltOnErrorRules = false,
                ThrowExceptionOnError = throwExceptionOnError,
                RequestType = ValidationRequestType.FieldsNoCollectionHaltOnErrorRecursive
            };
        }

        public static IValidationRequest FieldsNoRecursion()
        {
            return FieldsNoRecursion(false);
        }

        public static IValidationRequest FieldsNoRecursion(bool throwExceptionOnError)
        {
            return new ValidationRequest
            {
                IsFields = true,
                IsRules = false,
                IsRecursiveFields = false,
                IsRecursiveCollectionFields = false,
                IsHaltOnErrorFields = false,
                IsRecursiveRules = false,
                IsRecursiveCollectionRules = false,
                IsHaltOnErrorRules = false,
                ThrowExceptionOnError = throwExceptionOnError,
                RequestType = ValidationRequestType.FieldsNoRecursion
            };
        }

        public static IValidationRequest RulesRecursive()
        {
            return RulesRecursive(false);
        }

        public static IValidationRequest RulesRecursive(bool throwExceptionOnError)
        {
            return new ValidationRequest
            {
                IsFields = false,
                IsRules = true,
                IsRecursiveFields = false,
                IsRecursiveCollectionFields = false,
                IsHaltOnErrorFields = false,
                IsRecursiveRules = true,
                IsRecursiveCollectionRules = true,
                IsHaltOnErrorRules = false,
                ThrowExceptionOnError = throwExceptionOnError,
                RequestType = ValidationRequestType.RulesRecursive
            };
        }

        public static IValidationRequest RulesHaltOnErrorRecursive()
        {
            return RulesHaltOnErrorRecursive(false);
        }

        public static IValidationRequest RulesHaltOnErrorRecursive(bool throwExceptionOnError)
        {
            return new ValidationRequest
            {
                IsFields = false,
                IsRules = true,
                IsRecursiveFields = false,
                IsRecursiveCollectionFields = false,
                IsHaltOnErrorFields = false,
                IsRecursiveRules = true,
                IsRecursiveCollectionRules = true,
                IsHaltOnErrorRules = true,
                ThrowExceptionOnError = throwExceptionOnError,
                RequestType = ValidationRequestType.RulesHaltOnErrorRecursive
            };
        }

        public static IValidationRequest RulesNoCollectionRecursive()
        {
            return RulesNoCollectionRecursive(false);
        }

        public static IValidationRequest RulesNoCollectionRecursive(bool throwExceptionOnError)
        {
            return new ValidationRequest
            {
                IsFields = false,
                IsRules = true,
                IsRecursiveFields = false,
                IsRecursiveCollectionFields = false,
                IsHaltOnErrorFields = false,
                IsRecursiveRules = true,
                IsRecursiveCollectionRules = false,
                IsHaltOnErrorRules = false,
                ThrowExceptionOnError = throwExceptionOnError,
                RequestType = ValidationRequestType.RulesNoCollectionRecursive
            };
        }

        public static IValidationRequest RulesNoCollectionHaltOnErrorRecursive()
        {
            return RulesNoCollectionHaltOnErrorRecursive(false);
        }

        public static IValidationRequest RulesNoCollectionHaltOnErrorRecursive(bool throwExceptionOnError)
        {
            return new ValidationRequest
            {
                IsFields = false,
                IsRules = true,
                IsRecursiveFields = false,
                IsRecursiveCollectionFields = false,
                IsHaltOnErrorFields = false,
                IsRecursiveRules = true,
                IsRecursiveCollectionRules = false,
                IsHaltOnErrorRules = true,
                ThrowExceptionOnError = throwExceptionOnError,
                RequestType = ValidationRequestType.RulesNoCollectionHaltOnErrorRecursive
            };
        }

        public static IValidationRequest RulesNoRecursion()
        {
            return RulesNoRecursion(false);
        }

        public static IValidationRequest RulesNoRecursion(bool throwExceptionOnError)
        {
            return new ValidationRequest
            {
                IsFields = false,
                IsRules = true,
                IsRecursiveFields = false,
                IsRecursiveCollectionFields = false,
                IsHaltOnErrorFields = false,
                IsRecursiveRules = false,
                IsRecursiveCollectionRules = false,
                IsHaltOnErrorRules = false,
                ThrowExceptionOnError = throwExceptionOnError,
                RequestType = ValidationRequestType.RulesNoRecursion
            };
        }
        #endregion
    }
}

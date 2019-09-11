using System;

using Bedrock.Shared.Data.Validation.Enumeration;
using Bedrock.Shared.Data.Validation.Interface;

namespace Bedrock.Shared.Data.Validation.Implementation
{
    [Serializable]
    public class ValidationResult : IValidationResult
    {
        #region Static Fields
        public static readonly ValidationResult ValidField = new ValidationResult(ValidationResultType.Field);
        #endregion

        #region Fields
        private IPropertyDescriptor _property;
        private string _description;
        private ValidationResultSeverity _severity;
        private ValidationResultType _resultType;
        #endregion

        #region Constructor
        public ValidationResult(ValidationResultType resultType)
        {
            ResultType = resultType;
        }

        public ValidationResult(IPropertyDescriptor property, string description, ValidationResultType resultType) : this(property, description, ValidationResultSeverity.Error, resultType) { }

        public ValidationResult(string description, ValidationResultSeverity severity, ValidationResultType resultType) : this(null, description, severity, resultType) { }

        public ValidationResult(IPropertyDescriptor property, string description, ValidationResultSeverity severity, ValidationResultType resultType)
        {
            Property = property;
            Description = description;
            Severity = severity;
            ResultType = resultType;
        }
        #endregion

        #region Properties
        public IPropertyDescriptor Property
        {
            get { return _property; }
            set { _property = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public ValidationResultSeverity Severity
        {
            get { return _severity; }
            set { _severity = value; }
        }

        public ValidationResultType ResultType
        {
            get { return _resultType; }
            set { _resultType = value; }
        }
        #endregion
    }
}

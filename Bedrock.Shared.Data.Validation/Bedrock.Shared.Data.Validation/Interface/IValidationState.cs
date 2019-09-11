using Bedrock.Shared.Data.Validation.Enumeration;
using System.Collections.Generic;

namespace Bedrock.Shared.Data.Validation.Interface
{
    public interface IValidationState
    {
        #region Properties
        ValidationRequestType RequestType { get; set; }

        IValidatableEntity ValidatableEntity { get; }

        bool IsValidFields { get; }

        bool IsSelfValidFields { get; }

        bool IsValidRules { get; }

        bool IsSelfValidRules { get; }

        bool IsValid { get; }

        IEnumerable<string> AllResultDescriptions { get; }

        IEnumerable<string> FieldResultDescriptions { get; }

        IEnumerable<string> RuleResultDescriptions { get; }

        string FieldValidationResultsText { get; }

        string RuleValidationResultsText { get; }

        string ValidationResultsText { get; }

        List<IValidationResult> FieldValidationResults { get; }

        List<IValidationResult> RuleValidationResults { get; }

        List<IValidationResult> AllValidationResults { get; }
        #endregion

        #region Methods
        IValidationResult AddValidationResult(string description, ValidationResultSeverity severity, ValidationResultType resultType, string propertyName = null);
        #endregion
    }
}

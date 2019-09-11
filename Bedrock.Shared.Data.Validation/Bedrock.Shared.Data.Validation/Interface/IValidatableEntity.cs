using System.Collections.Generic;
using DA = System.ComponentModel.DataAnnotations;

namespace Bedrock.Shared.Data.Validation.Interface
{
    public interface IValidatableEntity
    {
        #region Properties
        IFieldValidationManager FieldValidationManager { get; }

        IRuleValidationManager RuleValidationManager { get; }

        IValidationState ValidationState { get; }

        bool IsRoot { get; set; }

        List<DA.ValidationResult> ValidationResults { get; }
        #endregion

        #region Methods
        void Validate(IValidationConfiguration validationConfiguration);

        void ValidateFields(IValidationConfiguration validationConfiguration);

        void ValidateRules(IValidationConfiguration validationConfiguration);

        void SetValidationContext(IValidationContext validationContext);

        void Reset();

        void ResetFieldValidationManager();

        void ResetRuleValidationManager();

        void ResetValidationContext();

        void ClearValidationResults();
        #endregion
    }
}

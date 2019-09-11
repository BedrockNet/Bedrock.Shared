using System;
using System.Collections.Generic;

namespace Bedrock.Shared.Data.Validation.Interface
{
    public interface IFieldValidationManager
    {
        #region Events
        event EventHandler<EventArgs> IsValidChanged;

        event EventHandler<EventArgs> IsSelfValidChanged;
        #endregion

        #region Properties
        bool IsSelfValid { get; set; }

        bool IsValid { get; set; }

        List<IValidationResult> Results { get; }
        #endregion

        #region Methods
        void CheckRules();

        string GetBrokenRules();

        string GetBrokenRulesForProperty(string propertyName);

        void Reset();
        #endregion
    }
}

using System;
using System.Collections.Generic;

namespace Bedrock.Shared.Data.Validation.Interface
{
    public interface IValidationRuleProvider
    {
        #region Methods
        IEnumerable<IValidationRule> GetValidationRules(Type type);
        #endregion
    }
}

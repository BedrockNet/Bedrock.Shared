using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Bedrock.Shared.Data.Validation.Interface;

namespace Bedrock.Shared.Data.Validation.Implementation.Field
{
    public class ValidationRuleCache
    {
        #region Fields
        private IValidationRuleProvider _ruleProvider;
        private ConcurrentDictionary<Type, IEnumerable<IValidationRule>> _cachedRules;
        #endregion

        #region Constructor
        public ValidationRuleCache(IValidationRuleProvider ruleProvider)
        {
            _ruleProvider = ruleProvider;
            _cachedRules = new ConcurrentDictionary<Type, IEnumerable<IValidationRule>>();
        }
        #endregion

        #region Methods
        public IEnumerable<IValidationRule> GetValidationRules(Type type)
        {
            return _cachedRules.GetOrAdd(type, x => _ruleProvider.GetValidationRules(type));
        }
        #endregion
    }
}

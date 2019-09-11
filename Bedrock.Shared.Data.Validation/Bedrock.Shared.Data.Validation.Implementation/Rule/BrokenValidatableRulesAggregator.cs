using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Extension;
using Bedrock.Shared.Data.Validation.Interface;

using SharedUtility = Bedrock.Shared.Utility;

namespace Bedrock.Shared.Data.Validation.Implementation.Rule
{
    public class BrokenValidatableRulesAggregator
    {
        #region Fields
        private static string _validationResultFormat;
        #endregion

        #region Properties
        public static string ValidationResultFormat
        {
            get
            {
                if (string.IsNullOrEmpty(_validationResultFormat))
                    _validationResultFormat = SharedUtility.StringHelper.Current.Lookup(StringApplication.ValidationResultFormat);

                return _validationResultFormat;
            }

            set { _validationResultFormat = value; }
        }
        #endregion

        #region Static Methods
        public static string GetValidationResultsText(IValidatableEntity validatableEntity, IEnumerable<IValidationResult> customResults = null)
        {
            var returnValue = new StringBuilder();
            var validationResultIndex = 0;

            customResults?.Concat(GetValidationResults(validatableEntity, SharedUtility.ObjectVisitationHelper.CreateInstance())).Each(vr =>
            {
                returnValue.AppendFormat(ValidationResultFormat, vr.Description, validationResultIndex + 1, Environment.NewLine);
                validationResultIndex++;
            });

            return returnValue.ToString();
        }

        public static IValidationResult[] GetValidationResults(IValidatableEntity validatableEntity, SharedUtility.ObjectVisitationHelper visitationHelper)
        {
            var type = validatableEntity.GetType();
            var nonValidated = SharedUtility.ApplicationContext.DomainGraphCache.GetPropertiesAttributes<NonValidated>(type);
            var brokenRules = new List<IValidationResult>(validatableEntity.RuleValidationManager.Results);
            var children = new List<PropertyInfo>(SharedUtility.ApplicationContext.DomainGraphCache.GetPropertiesOfType<IValidatableEntity>(type).Where(x => !nonValidated.ContainsKey(x)));

            children.AddRange(new List<PropertyInfo>(SharedUtility.ApplicationContext.DomainGraphCache.GetPropertiesOfType<IEnumerable<IValidatableEntity>>(type).Where(x => !nonValidated.ContainsKey(x))));

            visitationHelper.TryVisit(validatableEntity);

            foreach (var child in children)
            {
                var childValue = child.GetValue(validatableEntity, null);

                if (childValue is IEnumerable<IValidatableEntity>)
                {
                    var values = child.GetValue(validatableEntity, null) as IEnumerable<IValidatableEntity>;

                    foreach (var value in values)
                    {
                        if (value != null)
                        {
                            if (!visitationHelper.TryVisit(value))
                                continue;

                            brokenRules.AddRange(GetValidationResults(value, visitationHelper));
                        }
                    }
                }
                else
                {
                    var value = childValue as IValidatableEntity;

                    if (value != null)
                    {
                        if (!visitationHelper.TryVisit(value))
                            continue;

                        brokenRules.AddRange(GetValidationResults(value, visitationHelper));
                    }
                }
            }

            return brokenRules.ToArray();
        }
        #endregion
    }
}
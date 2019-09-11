using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Utility;
using Bedrock.Shared.Data.Validation.Enumeration;
using Bedrock.Shared.Data.Validation.Implementation.Field;
using Bedrock.Shared.Data.Validation.Implementation.Rule;
using Bedrock.Shared.Data.Validation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bedrock.Shared.Data.Validation.Implementation
{
    [Serializable]
    public class ValidationState : IValidationState
    {
        #region Constructors
        public ValidationState(IValidatableEntity validatableEntity, IValidationContext validationContext, IFieldValidationManager fieldValidationManager, IRuleValidationManager ruleValidationManager)
        {
            ValidatableEntity = validatableEntity;
            ValidationContext = validationContext;
            FieldValidationManager = fieldValidationManager;
            RuleValidationManager = ruleValidationManager;

            ValidationStateValidationResults = new List<IValidationResult>();
        }
        #endregion

        #region Public Properties
        public ValidationRequestType RequestType { get; set; }

        public IValidatableEntity ValidatableEntity { get; private set; }

        public bool IsValidFields
        {
            get { return !ValidationStateValidationResultsField.Any() & FieldValidationManager.IsValid; }
        }

        public bool IsSelfValidFields
        {
            get { return FieldValidationManager.IsSelfValid; }
        }

        public bool IsValidRules
        {
            get { return !ValidationStateValidationResultsRule.Any() & RuleValidationManager.IsValid; }
        }

        public bool IsSelfValidRules
        {
            get { return RuleValidationManager.IsSelfValid; }
        }

        public bool IsValid
        {
            get { return IsValidFields && IsValidRules; }
        }

        public IEnumerable<string> AllResultDescriptions
        {
            get
            {
                return FieldResultDescriptions
                        .Concat(RuleResultDescriptions);
            }
        }

        public IEnumerable<string> FieldResultDescriptions
        {
            get
            {
                return FieldValidationResults
                        .Select(r => r.Description);
            }
        }

        public IEnumerable<string> RuleResultDescriptions
        {
            get
            {
                return RuleValidationResults
                        .Select(r => r.Description);
            }
        }

        public string FieldValidationResultsText
        {
            get
            {
                return StringHelper.Current.Lookup(StringApplication.FieldValidationResults, BrokenFieldRulesAggregator.GetValidationResultsText(ValidatableEntity, ValidationStateValidationResultsField));
            }
        }

        public string RuleValidationResultsText
        {
            get
            {
                return StringHelper.Current.Lookup(StringApplication.RuleValidationResults, BrokenValidatableRulesAggregator.GetValidationResultsText(ValidatableEntity, ValidationStateValidationResultsRule));
            }
        }

        public string ValidationResultsText
        {
            get
            {
                return string.Concat(FieldValidationResultsText, Environment.NewLine, RuleValidationResultsText).TrimStart(Environment.NewLine.ToCharArray());
            }
        }

        public List<IValidationResult> FieldValidationResults
        {
            get
            {
                var validationResults = BrokenFieldRulesAggregator.GetValidationResults(ValidatableEntity, ObjectVisitationHelper.CreateInstance());
                return new List<IValidationResult>(ValidationStateValidationResultsField.Concat(validationResults));
            }
        }

        public List<IValidationResult> RuleValidationResults
        {
            get
            {
                var validationResults = BrokenValidatableRulesAggregator.GetValidationResults(ValidatableEntity, ObjectVisitationHelper.CreateInstance());
                return new List<IValidationResult>(ValidationStateValidationResultsRule.Concat(validationResults));
            }
        }

        public List<IValidationResult> AllValidationResults
        {
            get
            {
                var returnValue = new List<IValidationResult>();

                returnValue.AddRange(FieldValidationResults);
                returnValue.AddRange(RuleValidationResults);

                return returnValue;
            }
        }
        #endregion

        #region Protected Properties
        protected IValidationContext ValidationContext { get; private set; }

        protected IFieldValidationManager FieldValidationManager { get; private set; }

        protected IRuleValidationManager RuleValidationManager { get; private set; }

        protected List<IValidationResult> ValidationStateValidationResults { get; private set; }

        protected IEnumerable<IValidationResult> ValidationStateValidationResultsField
        {
            get { return ValidationStateValidationResults.Where(r => r.ResultType == ValidationResultType.Field); }
        }

        protected IEnumerable<IValidationResult> ValidationStateValidationResultsRule
        {
            get { return ValidationStateValidationResults.Where(r => r.ResultType == ValidationResultType.Rule); }
        }
        #endregion

        #region Public Methods
        public IValidationResult AddValidationResult(string description, ValidationResultSeverity severity, ValidationResultType resultType, string propertyName = null)
        {
            var property = new PropertyDescriptor(propertyName);
            var validationResult = new ValidationResult(property, description, severity, resultType);

            ValidationStateValidationResults.Add(validationResult);

            return validationResult;
        }
        #endregion
    }
}

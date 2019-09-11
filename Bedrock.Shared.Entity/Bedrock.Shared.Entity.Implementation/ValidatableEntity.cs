using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

using Bedrock.Shared.Data.Validation.Implementation;
using Bedrock.Shared.Data.Validation.Implementation.Field;
using Bedrock.Shared.Data.Validation.Implementation.Rule;
using Bedrock.Shared.Data.Validation.Interface;

using ValidationImplementation = Bedrock.Shared.Data.Validation.Implementation;
using DA = System.ComponentModel.DataAnnotations;

namespace Bedrock.Shared.Entity.Implementation
{
    [Serializable]
    public abstract class ValidatableEntity<TEntity> : EntityBase<TEntity>, IValidatableEntity
        where TEntity : ValidatableEntity<TEntity>, IValidatableEntity
    {
        #region Member Fields
        private IFieldValidationManager _fieldValidationManager;
        private IRuleValidationManager _ruleValidationManager;
        private IValidationContext _validationContext;
        private IValidationState _validationState;

        [NonSerialized]
        private List<DA.ValidationResult> _validationResults;
        #endregion

        #region Constructors
        public ValidatableEntity()
        {
            ValidationResults = new List<DA.ValidationResult>();
        }
        #endregion

        #region Public Properties
        [XmlIgnore]
        [NotMapped]
        public bool IsRoot { get; set; }
        #endregion

        #region IValidatableEntity Properties
        [XmlIgnore]
        [NotMapped]
        public IFieldValidationManager FieldValidationManager
        {
            get
            {
                if (_fieldValidationManager == null)
                    _fieldValidationManager = new FieldValidationManager(this, ValidationContext);

                return _fieldValidationManager;
            }
        }

        [XmlIgnore]
        [NotMapped]
        public IRuleValidationManager RuleValidationManager
        {
            get
            {
                if (_ruleValidationManager == null)
                    _ruleValidationManager = new RuleValidationManager(this, ValidationContext);

                return _ruleValidationManager;
            }
        }

        [XmlIgnore]
        [NotMapped]
        public IValidationState ValidationState
        {
            get
            {
                if (_validationState == null)
                    _validationState = new ValidationState(this, ValidationContext, FieldValidationManager, RuleValidationManager);

                return _validationState;
            }
        }

        [XmlIgnore]
        [NotMapped]
        public List<DA.ValidationResult> ValidationResults
        {
            get { return _validationResults; }
            private set { _validationResults = value; }
        }
        #endregion

        #region Protected Properties
        [XmlIgnore]
        [NotMapped]
        protected IValidationContext ValidationContext
        {
            get
            {
                if (_validationContext == null)
                {
                    _validationContext = ValidationImplementation.ValidationContext.CreateInstance();
                    IsRoot = true;
                }

                return _validationContext;
            }

            set { _validationContext = value; }
        }
        #endregion

        #region IValidatableEntity Methods
        public void Validate(IValidationConfiguration validationConfiguration)
        {
            ValidationContext.SetConfiguration(validationConfiguration);
            ValidationState.RequestType = validationConfiguration.Request.RequestType;

            if (validationConfiguration.Request.IsFields)
                ValidateFields(validationConfiguration);

            ValidationContext.Reset(this);

            if (validationConfiguration.Request.IsRules)
                ValidateRules(validationConfiguration);
        }

        public void ValidateFields(IValidationConfiguration validationConfiguration)
        {
            ValidationContext.SetConfiguration(validationConfiguration);
            FieldValidationManager.CheckRules();
        }

        public void ValidateRules(IValidationConfiguration validationConfiguration)
        {
            ValidationContext.SetConfiguration(validationConfiguration);
            RuleValidationManager.CheckRules();
        }

        public void SetValidationContext(IValidationContext validationContext)
        {
            ValidationContext = validationContext;
        }

        public void Reset()
        {
            ResetFieldValidationManager();
            ResetRuleValidationManager();
            ResetValidationContext();
        }

        public void ResetFieldValidationManager()
        {
            FieldValidationManager.Reset();
        }

        public void ResetRuleValidationManager()
        {
            RuleValidationManager.Reset();
        }

        public void ResetValidationContext()
        {
            ValidationContext.Reset();
        }

        public virtual void ClearValidationResults()
        {
            ValidationResults.Clear();
        }
        #endregion
    }       
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Bedrock.Shared.Extension;
using Bedrock.Shared.Data.Validation.Enumeration;
using Bedrock.Shared.Data.Validation.Interface;

using SharedUtility = Bedrock.Shared.Utility;

namespace Bedrock.Shared.Data.Validation.Implementation.Field
{
    [Serializable]
    public class FieldValidationManager : IFieldValidationManager
    {
        #region Fields
        private IValidatableEntity _validatableEntity;
        private List<IValidationRule> _validationRules;
        private List<IValidationResult> _results;
        private List<PropertyInfo> _children;
        private bool _isValid = true;
        private bool _isSelfValid = true;
        #endregion

        #region Constructor
        public FieldValidationManager(IValidatableEntity validatableEntity, IValidationContext validationContext)
        {
            if (validatableEntity == null)
                throw new ArgumentNullException(nameof(IValidatableEntity));

            _validatableEntity = validatableEntity;
            var type = validatableEntity.GetType();

            _validationRules = new List<IValidationRule>(ApplicationContext.ValidationRuleCache.GetValidationRules(type));
            var nonValidated = SharedUtility.ApplicationContext.DomainGraphCache.GetPropertiesAttributes<NonValidated>(type);

            _children = new List<PropertyInfo>(SharedUtility.ApplicationContext.DomainGraphCache.GetPropertiesOfType<IValidatableEntity>(type).Where(x => !nonValidated.ContainsKey(x)));
            _children.AddRange(new List<PropertyInfo>(SharedUtility.ApplicationContext.DomainGraphCache.GetPropertiesOfType<IEnumerable<IValidatableEntity>>(type).Where(x => !nonValidated.ContainsKey(x))));

            _results = new List<IValidationResult>();

            SetValidationContext(validationContext);
        }
        #endregion

        #region Public Events
        public event EventHandler<EventArgs> IsValidChanged;
        #endregion

        #region Public Events
        public event EventHandler<EventArgs> IsSelfValidChanged;
        #endregion

        #region Public Properties
        public bool IsSelfValid
        {
            get { return _isSelfValid; }
            set
            {
                if (_isSelfValid != value)
                {
                    _isSelfValid = value;
                    OnIsSelfValidChanged();
                }
            }
        }

        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnIsValidChanged();
                }
            }
        }

        public List<IValidationResult> Results
        {
            get { return _results; }
        }

        protected IValidationContext ValidationContext { get; set; }
        #endregion

        #region Public Methods
        public void CheckRules()
        {
            _validationRules.Each(vr =>
            {
                var result = vr.Execute(_validatableEntity);

                if (!result.Equals(ValidationResult.ValidField))
                    _results.Add(result);
            });

            if (ValidationContext.Configuration.TypeRules != null && ValidationContext.Configuration.TypeRules.Any())
            {
                var type = _validatableEntity.GetType();

                if (ValidationContext.Configuration.TypeRules.ContainsKey(type))
                {
                    ValidationContext.Configuration.TypeRules[type].EntityRules.Each(er =>
                    {
                        var result = er.Execute(_validatableEntity);

                        if (!result.Equals(ValidationResult.ValidField))
                            _results.Add(result);
                    });

                    ValidationContext.Configuration.TypeRules[type].EntityRuleExpressions.Each(ere =>
                    {
                        if (!ere.Evaluate(_validatableEntity))
                            _results.Add(new ValidationResult(ere.ErrorMessage, ValidationResultSeverity.Error, ValidationResultType.Field));
                    });
                }
            }

            IsSelfValid = _results.Count == 0;

            if (!IsSelfValid)
                ExceptionCheck(_validatableEntity);

            var request = ValidationContext.Configuration.Request;
            var isChildrenValid = request.IsRecursiveFields && (IsSelfValid || (!IsSelfValid && !request.IsHaltOnErrorFields)) ? CheckChildren() : true;

            IsValid = IsSelfValid & isChildrenValid;
        }

        public string GetBrokenRules()
        {
            var errorBuilder = new StringBuilder();
            _results.Each(vr => errorBuilder.AppendLine(vr.Description));
            return errorBuilder.ToString();
        }

        public string GetBrokenRulesForProperty(string propertyName)
        {
            var relevantRules = _results.Where(x => x.Property.Name == propertyName);
            var errorBuilder = new StringBuilder();

            relevantRules.Each(rr => errorBuilder.AppendLine(rr.Description));

            return errorBuilder.ToString();
        }

        public void Reset()
        {
            _isValid = true;
            _isSelfValid = true;

            Results.Clear();
        }
        #endregion

        #region Protected Methods
        protected void OnIsValidChanged()
        {
            IsValidChanged?.Invoke(this, new EventArgs());
        }

        protected void OnIsSelfValidChanged()
        {
            IsSelfValidChanged?.Invoke(this, new EventArgs());
        }
        #endregion

        #region Private Methods
        private bool CheckChildren()
        {
            var isValid = true;
            var request = ValidationContext.Configuration.Request;

            foreach (var child in _children)
            {
                var childValue = child.GetValue(_validatableEntity, null);

                if (childValue is IEnumerable<IValidatableEntity>)
                {
                    if (!request.IsRecursiveFields || !request.IsRecursiveCollectionFields)
                        continue;

                    var values = child.GetValue(_validatableEntity, null) as IEnumerable<IValidatableEntity>;

                    foreach (var value in values)
                    {
                        if (!CheckChild(value))
                        {
                            isValid = false;

                            ExceptionCheck(value);

                            if (request.IsHaltOnErrorFields)
                                return isValid;
                        }
                    }
                }
                else
                {
                    var value = childValue as IValidatableEntity;

                    if (!CheckChild(value))
                    {
                        isValid = false;

                        ExceptionCheck(value);

                        if (request.IsHaltOnErrorFields)
                            return isValid;
                    }
                }
            }

            return isValid;
        }

        private bool CheckChild(IValidatableEntity child)
        {
            var returnValue = true;

            if (child == null)
                return returnValue;

            if (child != null)
            {
                if (!ValidationContext.Update(child))
                    return returnValue;

                if (ValidationContext.Configuration.Request.IsRecursiveFields)
                {
                    child.ValidateFields(ValidationContext.Configuration);

                    if (!child.FieldValidationManager.IsValid)
                        return false;
                }
            }

            return returnValue;
        }

        private void SetValidationContext(IValidationContext validationContext)
        {
            ValidationContext = validationContext;
            ValidationContext.Update(_validatableEntity);
        }

        private void ExceptionCheck(IValidatableEntity entity)
        {
            if (ValidationContext.Configuration.Request.ThrowExceptionOnError)
                throw new Exception(entity.ValidationState.FieldValidationResultsText);
        }
        #endregion
    }
}

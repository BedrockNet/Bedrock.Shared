using System;

using Bedrock.Shared.Enumeration.StringHelper;

using Bedrock.Shared.Service.Interface;
using Bedrock.Shared.Session.Interface;

using Bedrock.Shared.Data.Validation.Implementation;
using Bedrock.Shared.Data.Validation.Implementation.Configuration;
using Bedrock.Shared.Data.Validation.Interface;

using Bedrock.Shared.Utility;

namespace Bedrock.Shared.Service.Implementation
{
    public abstract class ServiceBaseValidation : ServiceBase
    {
        #region Constructors
        public ServiceBaseValidation(params ISessionAware[] sessionAwareDependencies) : base(sessionAwareDependencies) { }
        #endregion

        #region Protected Methods (Response)
        protected IServiceResponse<TEntity, TEntity> ResponseFullValidation<TEntity>(TEntity entity, IValidationConfiguration validationConfiguration = null)
            where TEntity : class
        {
            return ResponseValidation(entity, EnsureConfiguration(validationConfiguration, ValidationRequest.Full()));
        }

        protected IServiceResponse<TEntity, TEntity> ResponseFieldValidation<TEntity>(TEntity entity, IValidationConfiguration validationConfiguration = null)
            where TEntity : class
        {
            return ResponseValidation(entity, EnsureConfiguration(validationConfiguration, ValidationRequest.FieldsRecursive()));
        }

        protected IServiceResponse<TEntity, TEntity> ResponseRuleValidation<TEntity>(TEntity entity, IValidationConfiguration validationConfiguration = null)
            where TEntity : class
        {
            return ResponseValidation(entity, EnsureConfiguration(validationConfiguration, ValidationRequest.RulesRecursive()));
        }

        protected IServiceResponse<TEntity, TEntity> ResponseValidation<TEntity>(TEntity entity, IValidationConfiguration validationConfiguration = null)
            where TEntity : class
        {
            var validationState = Validate((IValidatableEntity)entity, EnsureConfiguration(validationConfiguration));
            return Response(entity, validationState);
        }

        protected IServiceResponse<TEntity, TContract> ResponseFullValidation<TEntity, TContract>(TEntity entity, TContract contract, IValidationConfiguration validationConfiguration = null)
            where TEntity : class
            where TContract : class
        {
            return ResponseValidation(entity, contract, EnsureConfiguration(validationConfiguration, ValidationRequest.Full()));
        }

        protected IServiceResponse<TEntity, TContract> ResponseFieldValidation<TEntity, TContract>(TEntity entity, TContract contract, IValidationConfiguration validationConfiguration = null)
            where TEntity : class
            where TContract : class
        {
            return ResponseValidation(entity, contract, EnsureConfiguration(validationConfiguration, ValidationRequest.FieldsRecursive()));
        }

        protected IServiceResponse<TEntity, TContract> ResponseRuleValidation<TEntity, TContract>(TEntity entity, TContract contract, IValidationConfiguration validationConfiguration = null)
            where TEntity : class
            where TContract : class
        {
            return ResponseValidation(entity, contract, EnsureConfiguration(validationConfiguration, ValidationRequest.RulesRecursive()));
        }

        protected IServiceResponse<TEntity, TContract> ResponseValidation<TEntity, TContract>(TEntity entity, TContract contract, IValidationConfiguration validationConfiguration = null)
            where TEntity : class
            where TContract : class
        {
            var validationState = Validate((IValidatableEntity)entity, EnsureConfiguration(validationConfiguration));
            return Response(entity, contract, validationState);
        }
        #endregion

        #region Protected Methods (Validation)
        protected IValidationState Validate(IValidatableEntity validatableEntity, IValidationConfiguration validationConfiguration = null)
        {
            return Validate(validatableEntity, false, EnsureConfiguration(validationConfiguration));
        }

        protected IValidationState Validate(IValidatableEntity validatableEntity, bool throwError, IValidationConfiguration validationConfiguration = null)
        {
            validatableEntity.Validate(EnsureConfiguration(validationConfiguration));

            if (!validatableEntity.ValidationState.IsValid)
                if (throwError) ThrowValidationException(validatableEntity);

            return validatableEntity.ValidationState;
        }
        #endregion

        #region Private Methods
        private void ThrowValidationException(IValidatableEntity validatableEntity)
        {
            var exception = new Exception(validatableEntity.ValidationState.ValidationResultsText);
            Logger.Error(exception, StringHelper.Current.Lookup(StringError.InvalidFieldsOrRules));
        }

        private IValidationConfiguration EnsureConfiguration(IValidationConfiguration configuration, IValidationRequest request = null)
        {
            var returnValue = configuration != null ? configuration : new ValidationConfiguration();

            if (request != null)
                returnValue.Request = request;

            return returnValue;
        }
        #endregion
    }
}

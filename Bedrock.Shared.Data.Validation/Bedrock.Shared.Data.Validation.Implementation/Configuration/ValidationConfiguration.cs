using System;
using System.Collections.Concurrent;

using Bedrock.Shared.Data.Validation.Interface;

namespace Bedrock.Shared.Data.Validation.Implementation.Configuration
{
    public class ValidationConfiguration : IValidationConfiguration
    {
        #region Constructors
        public ValidationConfiguration() : this(ValidationRequest.Full()) { }
        
        public ValidationConfiguration(IValidationRequest validationRequest)
        {
            Request = validationRequest != null ? validationRequest : ValidationRequest.Full();
            TypeRules = new ConcurrentDictionary<Type, IValidationConfigurationRules>();
        }
        #endregion

        #region Properties
        public IValidationRequest Request { get; set; }

        public ConcurrentDictionary<Type, IValidationConfigurationRules> TypeRules { get; set; }
        #endregion

        #region Public Methods
        public IValidationConfigurationRules<TEntity> CreateRules<TEntity>() where TEntity : class
        {
            return (IValidationConfigurationRules<TEntity>)TypeRules.GetOrAdd(typeof(TEntity), new ValidationConfigurationRules<TEntity>());
        }
        #endregion
    }
}

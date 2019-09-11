using System;
using System.Collections.Concurrent;

namespace Bedrock.Shared.Data.Validation.Interface
{
    public interface IValidationConfiguration
    {
        #region Properties
        IValidationRequest Request { get; set; }

        ConcurrentDictionary<Type, IValidationConfigurationRules> TypeRules { get; set; }
        #endregion

        #region Methods
        IValidationConfigurationRules<TEntity> CreateRules<TEntity>() where TEntity : class;
        #endregion
    }
}

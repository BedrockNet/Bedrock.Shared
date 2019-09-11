using System;
using Bedrock.Shared.Data.Validation.Implementation.Field;

using CommonServiceLocator;
using DA = System.ComponentModel.DataAnnotations;

namespace Bedrock.Shared.Data.Validation.Implementation
{
    public static class ApplicationContext
    {
        #region Fields
        private static ValidationRuleCache _validationRuleCache;
        #endregion

        #region Properties
        public static ValidationRuleCache ValidationRuleCache
        {
            get
            {
                if (_validationRuleCache == null)
                    _validationRuleCache = new ValidationRuleCache(new DataAnnotationsValidationRuleProvider());

                return _validationRuleCache;
            }
        }
        #endregion

        #region Public Methods
        public static DA.ValidationContext GetValidationContext(object entity)
        {
            var serviceProvider = ServiceLocator.Current.GetInstance<IServiceProvider>();
            return new DA.ValidationContext(entity, serviceProvider, null);
        }
        #endregion
    }
}

using System;

using Bedrock.Shared.Utility;
using Bedrock.Shared.Data.Validation.Interface;

namespace Bedrock.Shared.Data.Validation.Implementation
{
    [Serializable]
    public class ValidationContext : IValidationContext
    {
        #region Fields
        private ObjectVisitationHelper _objectVisitationHelper;
        #endregion

        #region Constructors
        private ValidationContext() { }
        #endregion

        #region Properties
        public ObjectVisitationHelper ObjectVisitationHelper
        {
            get
            {
                if (_objectVisitationHelper == null)
                    _objectVisitationHelper = ObjectVisitationHelper.CreateInstance();

                return _objectVisitationHelper;
            }
        }

        public IValidationConfiguration Configuration { get; private set; }
        #endregion

        #region Public Methods
        public static ValidationContext CreateInstance()
        {
            var returnValue = new ValidationContext();
            return returnValue;
        }

        public bool Update(IValidatableEntity validatableEntity)
        {
            validatableEntity.SetValidationContext(this);
            return ObjectVisitationHelper.TryVisit(validatableEntity);
        }

        public void SetConfiguration(IValidationConfiguration validationConfiguration)
        {
            Configuration = validationConfiguration;
        }

        public void Reset(IValidatableEntity validatableEntity = null)
        {
            _objectVisitationHelper.Reset();

            if (validatableEntity != null)
                ObjectVisitationHelper.TryVisit(validatableEntity);

            SetConfiguration(null);
        }
        #endregion
    }
}

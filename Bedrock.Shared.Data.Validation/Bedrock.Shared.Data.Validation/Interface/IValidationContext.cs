using Bedrock.Shared.Utility;

namespace Bedrock.Shared.Data.Validation.Interface
{
    public interface IValidationContext
    {
        #region Properties
        ObjectVisitationHelper ObjectVisitationHelper { get; }

        IValidationConfiguration Configuration { get; }
        #endregion

        #region Methods
        bool Update(IValidatableEntity validatableEntity);

        void SetConfiguration(IValidationConfiguration validationConfiguration);

        void Reset(IValidatableEntity validatableEntity = null);
        #endregion
    }
}

using Bedrock.Shared.Data.Validation.Interface;

namespace Bedrock.Shared.Service.Interface
{
    public interface IServiceResponse<TEntity, TContract>
        where TEntity : class
        where TContract : class
    {
        #region Properties
        TEntity Entity { get; set; }

        TContract Contract { get; set; }

        IValidationState ValidationState { get; set; }
        #endregion

        #region Methods
        void PostSave();
        #endregion
    }
}

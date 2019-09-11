using Bedrock.Shared.Service.Interface;
using Bedrock.Shared.Utility;
using Bedrock.Shared.Data.Validation.Interface;

namespace Bedrock.Shared.Service.Implementation.Response
{
    public class ServiceResponse<TEntity, TContract> : ServiceResponseBase<TEntity, TContract>
        where TEntity : class
        where TContract : class
    {
        #region Constructors
        public ServiceResponse(TEntity entity, TContract contract) : this(entity, contract, null) { }

        public ServiceResponse(TEntity entity, TContract contract, IValidationState validationState)
        {
            Entity = entity;
            Contract = contract;
            ValidationState = validationState;
        }
        #endregion

        #region Public Static Methods
        public static IServiceResponse<TEntity, TContract> Create(TContract contract)
        {
            return Create(default, contract);
        }

        public static IServiceResponse<TEntity, TContract> Create(TEntity entity)
        {
            return Create(entity, default(TContract));
        }

        public static IServiceResponse<TEntity, TContract> Create(TEntity entity, TContract contract)
        {
            return new ServiceResponse<TEntity, TContract>(entity, contract);
        }

        public static IServiceResponse<TEntity, TContract> Create(TEntity entity, IValidationState validationState)
        {
            return Create(entity, default, validationState);
        }

        public static IServiceResponse<TEntity, TContract> Create(TContract contract, IValidationState validationState)
        {
            return Create(default, contract, validationState);
        }

        public static IServiceResponse<TEntity, TContract> Create(TEntity entity, TContract contract, IValidationState validationState)
        {
            return new ServiceResponse<TEntity, TContract>(entity, contract, validationState);
        }
        #endregion

        #region IServiceResponse Methods
        public override void PostSave()
        {
            SetContractFields(Entity, Contract, ObjectVisitationHelper.CreateInstance());
        }
        #endregion
    }
}

using System.Linq;
using System.Reflection;

using Bedrock.Shared.Cache.Interface;
using Bedrock.Shared.Extension;
using Bedrock.Shared.Log.Interface;
using Bedrock.Shared.Mapper.Interface;

using Bedrock.Shared.Service.Implementation.Response;
using Bedrock.Shared.Service.Interface;

using Bedrock.Shared.Session.Implementation;
using Bedrock.Shared.Session.Interface;

using Bedrock.Shared.Data.Validation.Interface;

namespace Bedrock.Shared.Service.Implementation
{
    public abstract class ServiceBase : SessionAwareBase, IService, ISessionAware
    {
        #region Constructors
        public ServiceBase() { }

        public ServiceBase(params ISessionAware[] sessionAwareDependencies) : base(sessionAwareDependencies)
        {
            SetServiceProperties(sessionAwareDependencies);
        }
        #endregion

        #region Properties
        public ILogger Logger { get; set; }

        public IMapper Mapper { get; set; }

        public ICacheProvider Cache { get; set; }
        #endregion

        #region Protected Methods (Response)
        protected IServiceResponse<TEntity, TEntity> Response<TEntity>(TEntity entity)
            where TEntity : class
        {
            return ServiceResponse<TEntity, TEntity>.Create(entity, entity);
        }

        protected IServiceResponse<TEntity, TContract> Response<TEntity, TContract>(TEntity entity, TContract contract)
            where TEntity : class
            where TContract : class
        {
            return ServiceResponse<TEntity, TContract>.Create(entity, contract);
        }

        protected IServiceResponse<TEntity, TEntity> Response<TEntity>(TEntity entity, IValidationState validationState)
            where TEntity : class
        {
            return Response(entity, entity, validationState);
        }

        protected IServiceResponse<TEntity, TContract> Response<TEntity, TContract>(TEntity entity, TContract contract, IValidationState validationState)
            where TEntity : class
            where TContract : class
        {
            return ServiceResponse<TEntity, TContract>.Create(entity, contract, validationState);
        }
        #endregion

        #region Protected Methods
        protected void SetServiceProperties(ISessionAware[] dependencies)
        {
            if (dependencies == null || !dependencies.Any())
                return;

            var flags = BindingFlags.Instance | BindingFlags.NonPublic;

            GetType()
                .GetProperties(flags)
                .Where(p => typeof(ISessionAware).IsAssignableFrom(p.PropertyType))
                .Each(p =>
                {
                    var serviceValue = dependencies
                                        .FirstOrDefault(d => p.PropertyType.IsAssignableFrom(d.GetType()));

                    if (serviceValue != null)
                        p.SetValue(this, serviceValue);
                });
        }
        #endregion
    }
}

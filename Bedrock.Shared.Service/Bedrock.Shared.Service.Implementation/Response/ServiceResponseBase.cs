using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Bedrock.Shared.Entity.Interface;
using Bedrock.Shared.Extension;
using Bedrock.Shared.Service.Interface;
using Bedrock.Shared.Utility;
using Bedrock.Shared.Data.Validation.Interface;

namespace Bedrock.Shared.Service.Implementation.Response
{
    public abstract class ServiceResponseBase<TEntity, TContract> : IServiceResponse<TEntity, TContract>
        where TEntity : class
        where TContract : class
    {
        #region IServiceResponse Properties
        public virtual TEntity Entity { get; set; }

        public virtual TContract Contract { get; set; }

        public virtual IValidationState ValidationState { get; set; }
        #endregion

        #region IServiceResponse Methods
        public abstract void PostSave();
        #endregion

        #region Protected Methods
        // This method assumes entity and contract have same shape
        protected void SetContractFields(object entity, object contract, ObjectVisitationHelper visitationHelper)
        {
            var isEntitySingular = typeof(IBedrockEntity).GetTypeInfo().IsAssignableFrom(entity.GetType());
            var isEntityCollection = typeof(IEnumerable<IBedrockEntity>).GetTypeInfo().IsAssignableFrom(entity.GetType());

            var isContractSingular = typeof(IBedrockEntity).GetTypeInfo().IsAssignableFrom(contract.GetType());
            var isContractCollection = typeof(IEnumerable<IBedrockEntity>).GetTypeInfo().IsAssignableFrom(contract.GetType());

            if (isEntitySingular && isContractSingular)
            {
                SetContractFieldsInternal(entity, contract, visitationHelper);
            }
            else if (isEntityCollection && isContractCollection)
            {
                var entities = entity as IEnumerable<IBedrockEntity>;
                var contracts = contract as IEnumerable<IBedrockEntity>;
                var contractList = new List<IBedrockEntity>(contracts);

                entities.Each((e, i) => SetContractFieldsInternal(e, contractList[i], visitationHelper));
            }
        }
        #endregion

        #region Private Methods
        // TODO:  Ensure entity and contract have same shape or do nothing; currently will fail otherwise
        private void SetContractFieldsInternal(object entity, object contract, ObjectVisitationHelper visitationHelper)
        {
            if (!visitationHelper.TryVisit(entity))
                return;

            SetFields(entity, contract);

            foreach (var propertyInfo in entity.GetType().GetTypeInfo().GetProperties())
            {
                if (propertyInfo.GetIndexParameters().Length > 0)
                    continue;

                var propertyValueEntity = propertyInfo.GetValue(entity, null);

                if (propertyValueEntity == null)
                    continue;

                var isEntitySingular = typeof(IBedrockEntity).GetTypeInfo().IsAssignableFrom(propertyInfo.PropertyType);
                var isEntityCollection = typeof(IEnumerable<IBedrockEntity>).GetTypeInfo().IsAssignableFrom(propertyInfo.PropertyType);

                if (isEntitySingular)
                {
                    var contractPropertyInfo = contract.GetType().GetTypeInfo().GetProperties().FirstOrDefault(p => p.Name == propertyInfo.Name);

                    if (contractPropertyInfo != null)
                    {
                        var propertyValueContract = contractPropertyInfo.GetValue(contract, null);

                        if (propertyValueContract != null)
                            SetContractFieldsInternal(propertyValueEntity, propertyValueContract, visitationHelper);
                    }
                }
                else if (isEntityCollection)
                {
                    var entities = propertyValueEntity as IEnumerable<IBedrockEntity>;
                    var contractPropertyInfo = contract.GetType().GetTypeInfo().GetProperties().FirstOrDefault(p => p.Name == propertyInfo.Name);
                    var contractPropertyCollection = contractPropertyInfo != null ? contractPropertyInfo.GetValue(contract, null) as IEnumerable<IBedrockEntity> : null;

                    if (contractPropertyCollection == null || (entities.Count() != contractPropertyCollection.Count()))
                        continue;

                    entities.Each((e, i) =>
                    {
                        var propertyValueContract = contractPropertyCollection.ElementAt(i);

                        if (propertyValueContract != null)
                            SetContractFieldsInternal(e, propertyValueContract, visitationHelper);
                    });
                }
            }
        }

        private void SetFields(object entity, object contract)
        {
            SetContractIdField(entity, contract);
            SetContractAuditFields(entity, contract);
            SetContractDeletableFields(entity, contract);
        }

        private void SetContractIdField(object entity, object contract)
        {
            if (contract == null || entity == null)
                return;

            var contractInterface = contract
                                    .GetType()
                                    .GetTypeInfo()
                                    .GetInterfaces()
                                    .FirstOrDefault(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IBedrockIdEntity<>));

            var entityInterface = entity
                                    .GetType()
                                    .GetTypeInfo()
                                    .GetInterfaces()
                                    .FirstOrDefault(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IBedrockIdEntity<>));

            if (contractInterface == null || entityInterface == null)
                return;

            var typeIdContract = contractInterface.GetTypeInfo().GetGenericArguments().First();
            var typeContract = typeof(IBedrockIdEntity<>).MakeGenericType(typeIdContract);

            var typeIdEntity = entityInterface.GetTypeInfo().GetGenericArguments().First();
            var typeEntity = typeof(IBedrockIdEntity<>).MakeGenericType(typeIdEntity);

            if (typeContract != null && typeEntity != null)
            {
                var idFieldName = contractInterface.GetTypeInfo().GetProperties().First().Name;
                var entityId = typeEntity.GetTypeInfo().GetProperty(idFieldName).GetValue(entity);

                typeContract.GetTypeInfo().GetProperty(idFieldName).SetValue(contract, entityId);
            }
        }

        private void SetContractAuditFields(object entity, object contract)
        {
            var contractAudit = contract as IBedrockAuditEntity;
            var entityAudit = entity as IBedrockAuditEntity;

            if (contractAudit != null && entityAudit != null)
            {
                contractAudit.CreatedBy = entityAudit.CreatedBy;
                contractAudit.CreatedDate = entityAudit.CreatedDate;
                contractAudit.UpdatedBy = entityAudit.UpdatedBy;
                contractAudit.UpdatedDate = entityAudit.UpdatedDate;
            }
        }

        private void SetContractDeletableFields(object entity, object contract)
        {
            var contractDeletable = contract as IBedrockDeletableEntity;
            var entityDeletable = entity as IBedrockDeletableEntity;

            if (contractDeletable != null && entityDeletable != null)
            {
                contractDeletable.IsDeleted = entityDeletable.IsDeleted;
                contractDeletable.DeletedBy = entityDeletable.DeletedBy;
                contractDeletable.DeletedDate = entityDeletable.DeletedDate;
            }
        }
        #endregion
    }
}

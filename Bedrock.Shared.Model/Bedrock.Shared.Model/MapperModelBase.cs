using System.Collections.Generic;
using System.Linq;

using Bedrock.Shared.Extension;
using Bedrock.Shared.Mapper.Interface;
using Bedrock.Shared.Utility;

using CommonServiceLocator;

namespace Bedrock.Shared.Model
{
    public abstract class MapperModelBase<DM, M> : ModelBase
        where DM : class, new()
        where M : MapperModelBase<DM, M>, new()
    {
        #region Fields
        private static IMapper _mapper;
        #endregion

        #region Properties
        private static IMapper Mapper
        {
            get
            {
                _mapper = _mapper ?? ServiceLocator.Current.GetInstance<IMapper>();
                return _mapper;
            }
        }
        #endregion

        #region Public Static Methods
        public static M StaticMapToModelFromDomainModel(DM domainModel)
        {
            if (domainModel == null)
                return null;

            var viewModel = New<M>.Instance();

            return MapToModelFromDomainModelInternal(viewModel, domainModel);
        }

        public static IEnumerable<M> StaticMapToModelsFromDomainModels(IEnumerable<DM> domainModels)
        {
            var returnValue = new List<M>();

            if (domainModels != null && domainModels.Count() > 0)
                domainModels.Each(dm => returnValue.Add(StaticMapToModelFromDomainModel(dm)));

            return returnValue;
        }

        public static DM StaticMapToDomainModelFromModel(M viewModel)
        {
            if (viewModel == null)
                return null;

            var domainModel = New<DM>.Instance();
            return MapToDomainModelFromModelInternal(domainModel, viewModel);
        }

        public static IEnumerable<DM> StaticMapToDomainModelsFromModels(IEnumerable<M> viewModels)
        {
            var returnValue = new List<DM>();

            if (viewModels != null && viewModels.Count() > 0)
                viewModels.Each(M => returnValue.Add(StaticMapToDomainModelFromModel(M)));

            return returnValue;
        }
        #endregion

        #region Public Methods
        public virtual M MapToModelFromDomainModel(DM domainModel)
        {
            return StaticMapToModelFromDomainModel(domainModel);
        }

        public virtual IEnumerable<M> MapToModelsFromDomainModels(IEnumerable<DM> domainModels)
        {
            return StaticMapToModelsFromDomainModels(domainModels);
        }

        public virtual DM MapToDomainModelFromModel(M viewModel)
        {
            return StaticMapToDomainModelFromModel(viewModel);
        }

        public virtual IEnumerable<DM> MapToDomainModelsFromModels(IEnumerable<M> viewModels)
        {
            return StaticMapToDomainModelsFromModels(viewModels);
        }
        #endregion

        #region Private Methods
        private static M MapToModelFromDomainModelInternal(M viewModel, DM domainModel)
        {
            return Mapper.Map<DM, M>(viewModel, domainModel);
        }

        private static DM MapToDomainModelFromModelInternal(DM domainModel, M viewModel)
        {
            return Mapper.Map<M, DM>(domainModel, viewModel);
        }
        #endregion
    }
}
using System.Linq;

using Bedrock.Shared.Extension;

using Bedrock.Shared.Mapper.AutoMapper.Interface;
using Bedrock.Shared.Mapper.Interface;

namespace Bedrock.Shared.Mapper.AutoMapper
{
    public class Mapper : IMapper
    {
        #region Fields
        private IMapperAutoMapper _mapperInternal;
        #endregion

        #region Constructors
        public Mapper() { }

        public Mapper(IMapperAutoMapper mapper)
        {
            MapperInternal = mapper;
        }
        #endregion

        #region Properties
        protected IMapperAutoMapper MapperInternal
        {
            get
            {
                _mapperInternal = _mapperInternal ?? new MapperAutoMapper();
                return _mapperInternal;
            }

            set { _mapperInternal = value; }
        }
        #endregion

        #region IMapper Methods
        public TTo Map<TFrom, TTo>(object to, params object[] from)
        {
            var returnValue = (TTo)to;
            var initialFrom = (TFrom)from[0];

            MapperInternal.Map(initialFrom, returnValue);

            if (from.Count() > 1)
                from.Skip(1)
                    .Each(f => MapperInternal.Map(f, returnValue));

            return returnValue;
        }

        public TTo Flatten<TFrom, TTo>(object to, params object[] from)
        {
            return Map<TFrom, TTo>(to, from);
        }

        public TTo Unflatten<TFrom, TTo>(object to, params object[] from)
        {
            return Map<TFrom, TTo>(to, from);
        }
        #endregion
    }
}

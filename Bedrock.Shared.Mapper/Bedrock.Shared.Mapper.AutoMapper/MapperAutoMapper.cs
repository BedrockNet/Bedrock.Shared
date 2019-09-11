using System;
using Bedrock.Shared.Mapper.AutoMapper.Interface;
using AM = AutoMapper;

namespace Bedrock.Shared.Mapper.AutoMapper
{
    public class MapperAutoMapper : IMapperAutoMapper
    {
        #region IMapperAutoMapper Methods
        public TTo Map<TTo>(object from)
        {
            return AM.Mapper.Map<TTo>(from);
        }

        public TTo Map<TFrom, TTo>(TFrom from)
        {
            return AM.Mapper.Map<TFrom, TTo>(from);
        }

        public TTo Map<TFrom, TTo>(TFrom from, TTo to)
        {
            return AM.Mapper.Map(from, to);
        }

        public object Map(object from, Type fromType, Type toType)
        {
            return AM.Mapper.Map(from, fromType, toType);
        }

        public object Map(object from, object to, Type fromType, Type toType)
        {
            return AM.Mapper.Map(from, to, fromType, toType);
        }
        #endregion
    }
}

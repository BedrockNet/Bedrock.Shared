using System;

namespace Bedrock.Shared.Mapper.AutoMapper.Interface
{
    public interface IMapperAutoMapper
    {
        #region Methods
        TTo Map<TTo>(object from);

        TTo Map<TFrom, TTo>(TFrom from);

        TTo Map<TFrom, TTo>(TFrom from, TTo to);

        object Map(object from, Type fromType, Type toType);

        object Map(object from, object to, Type fromType, Type toType);
        #endregion
    }
}

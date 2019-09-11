using System.Linq;
using Bedrock.Shared.Mapper.Interface;
using AutoMapper.QueryableExtensions;

namespace Bedrock.Shared.Mapper.AutoMapper
{
    public class MapperHelper : IMapperHelper
    {
        #region Public Methods
        public IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source)
        {
            return source.ProjectTo<TDestination>();
        }
        #endregion
    }
}

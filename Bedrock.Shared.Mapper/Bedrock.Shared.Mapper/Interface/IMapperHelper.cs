using System.Linq;

namespace Bedrock.Shared.Mapper.Interface
{
    public interface IMapperHelper
    {
        #region Methods
        IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source);
        #endregion
    }
}

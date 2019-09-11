using System.Linq;
using Bedrock.Shared.Mapper.Interface;
using CommonServiceLocator;

namespace Bedrock.Shared.Mapper.Extension
{
    public static class MapperExtension
    {
        #region Constructors
        static MapperExtension()
        {
            MapperHelper = ServiceLocator.Current.GetInstance<IMapperHelper>();
        }
        #endregion

        #region Properties
        private static IMapperHelper MapperHelper { get; set; }
        #endregion

        #region Public Methods
        public static IQueryable<TDestination> ProjectTo<TSource, TDestination>(this IQueryable<TSource> source)
        {
            return MapperHelper.ProjectTo<TSource, TDestination>(source);
        }
        #endregion
    }
}

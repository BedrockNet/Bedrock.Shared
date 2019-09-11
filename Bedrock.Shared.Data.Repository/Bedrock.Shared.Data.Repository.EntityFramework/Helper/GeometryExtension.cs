using System.Collections.Generic;

using GeoAPI;
using GeoAPI.Geometries;

using NetTopologySuite;
using NetTopologySuite.CoordinateSystems.Transformations;

using ProjNet;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace Bedrock.Shared.Data.Repository.EntityFramework.Helper
{
    public static class GeometryExtension
    {
        #region Fields
        private static readonly IGeometryServices _geometryServices = NtsGeometryServices.Instance;

        private static readonly ICoordinateSystemServices _coordinateSystemServices = new CoordinateSystemServices
        (
                new CoordinateSystemFactory(),
                new CoordinateTransformationFactory(),
                new Dictionary<int, string>
                {
                    // Coordinate systems:
                    // (3857 and 4326 included automatically)
                }
        );
        #endregion

        #region Public Methods
        public static IGeometry ProjectTo(this IGeometry geometry, int srid)
        {
            var geometryFactory = _geometryServices.CreateGeometryFactory(srid);
            var transformation = _coordinateSystemServices.CreateTransformation(geometry.SRID, srid);

            return GeometryTransform.TransformGeometry(geometryFactory, geometry, transformation.MathTransform);
        }
        #endregion
    }
}

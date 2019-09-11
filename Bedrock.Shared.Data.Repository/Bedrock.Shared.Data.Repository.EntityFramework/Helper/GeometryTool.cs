using Bedrock.Shared.Data.Repository.Interface;
using NetTopologySuite.Geometries;

namespace Bedrock.Shared.Data.Repository.EntityFramework.Helper
{
    public class GeometryTool : IGeometryTool
    {
        #region Public Methods
        public double CalculateDistance(GeometryPoint geometryPoint1, GeometryPoint geometryPoint2, int coordinateSystem = 3857)
        {
            var point1 = new Point(geometryPoint1.Longitude, geometryPoint1.Latitude) { SRID = geometryPoint1.Srid };
            var point2 = new Point(geometryPoint2.Longitude, geometryPoint2.Latitude) { SRID = geometryPoint2.Srid };

            return point1.ProjectTo(coordinateSystem).Distance(point2.ProjectTo(coordinateSystem));
        }
        #endregion
    }
}

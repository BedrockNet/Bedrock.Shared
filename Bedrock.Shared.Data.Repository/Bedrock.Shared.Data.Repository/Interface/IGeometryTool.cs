namespace Bedrock.Shared.Data.Repository.Interface
{
    public interface IGeometryTool
    {
        #region Methods
        double CalculateDistance(GeometryPoint geometryPoint1, GeometryPoint geometryPoint2, int coordinateSystem = 3857);
        #endregion
    }
}

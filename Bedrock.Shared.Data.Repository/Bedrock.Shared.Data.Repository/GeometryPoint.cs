namespace Bedrock.Shared.Data.Repository
{
    public class GeometryPoint
    {
        #region Constructors
        public GeometryPoint() { }

        public GeometryPoint(double latitude, double longitude) : this(latitude, longitude, 4326) { }

        public GeometryPoint(double latitude, double longitude, int srid)
        {
            Latitude = latitude;
            Longitude = longitude;
            Srid = srid;
        }
        #endregion

        #region Public Properties
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Srid { get; set; }
        #endregion
    }
}

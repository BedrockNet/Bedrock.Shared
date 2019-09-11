using Bedrock.Shared.Tools.VersionBumper.Bumper.Enumeration;
using Bedrock.Shared.Tools.VersionBumper.Bumper.Interface;

namespace Bedrock.Shared.Tools.VersionBumper.Bumper
{
    public abstract class BumperBase : IBumper
    {
        #region Constructors
        public BumperBase(BumperType bumperType, Configuration configuration)
        {
            BumperType = bumperType;
            Configuration = configuration;
        }
        #endregion

        #region IBumper Properties
        public BumperType BumperType { get; set; }
        #endregion

        #region Protected Properties
        public Configuration Configuration { get; set; }
        #endregion

        #region IBumper Methods
        public abstract string Bump(string currentVersion);
        #endregion
    }
}

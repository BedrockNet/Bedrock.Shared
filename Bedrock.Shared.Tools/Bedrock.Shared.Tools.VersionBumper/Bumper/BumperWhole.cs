using Bedrock.Shared.Tools.VersionBumper.Bumper.Enumeration;

namespace Bedrock.Shared.Tools.VersionBumper.Bumper
{
    public class BumperWhole : BumperBase
    {
        #region Constructors
        public BumperWhole(Configuration configuration) : base(BumperType.Whole, configuration) { }
        #endregion

        #region IBumper Methods
        public override string Bump(string currentVersion)
        {
            return Configuration.SpecificVersion;
        }
        #endregion
    }
}

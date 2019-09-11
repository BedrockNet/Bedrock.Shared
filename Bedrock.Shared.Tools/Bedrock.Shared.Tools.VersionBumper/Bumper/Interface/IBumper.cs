using Bedrock.Shared.Tools.VersionBumper.Bumper.Enumeration;

namespace Bedrock.Shared.Tools.VersionBumper.Bumper.Interface
{
    public interface IBumper
    {
        #region Properties
        BumperType BumperType { get; set; }
        #endregion

        #region Methods
        string Bump(string currentVersion);
        #endregion
    }
}

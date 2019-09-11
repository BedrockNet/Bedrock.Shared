using Bedrock.Shared.Tools.VersionBumper.Bumper.Enumeration;

namespace Bedrock.Shared.Tools.VersionBumper.Bumper.Interface
{
    public interface IBumperProvider
    {
        #region Methods
        string Bump(string currentVersion, BumperType bumperType = BumperType.Patch);
        #endregion
    }
}

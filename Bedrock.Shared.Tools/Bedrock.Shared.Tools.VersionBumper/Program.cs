namespace Bedrock.Shared.Tools.VersionBumper
{
    public class Program
    {
        #region Private Methods
        private static void Main(string[] arguments)
        {
            new VersionBumper().Bump(arguments);
        }
        #endregion
    }
}

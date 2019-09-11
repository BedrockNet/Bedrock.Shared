using System.Collections.Generic;
using System.Linq;

using Bedrock.Shared.Tools.VersionBumper.Bumper.Enumeration;
using Bedrock.Shared.Tools.VersionBumper.Bumper.Interface;

using Bedrock.Shared.Tools.VersionBumper.Logger;

namespace Bedrock.Shared.Tools.VersionBumper.Bumper
{
    public class BumperProvider : IBumperProvider
    {
        #region Constructors
        public BumperProvider(Configuration configuration, ILogger logger)
        {
            Initialize(configuration, logger);
        }
        #endregion

        #region Protected Properties
        protected List<IBumper> Bumpers { get; set; }

        protected Configuration Configuration { get; set; }

        protected ILogger Logger { get; set; }
        #endregion

        #region Public Methods
        public string Bump(string currentVersion, BumperType bumperType = BumperType.Patch)
        {
            return GetBumper(bumperType).Bump(currentVersion);
        }
        #endregion

        #region Private Methods
        private void Initialize(Configuration configuration, ILogger logger)
        {
            Configuration = configuration;
            Logger = logger;

            Bumpers = new List<IBumper>();

            Bumpers.Add(new BumperMajor(configuration));
            Bumpers.Add(new BumperMinor(configuration));
            Bumpers.Add(new BumperPatch(configuration));
            Bumpers.Add(new BumperWhole(configuration));
        }

        private IBumper GetBumper(BumperType bumperType)
        {
            return Bumpers.First(vb => vb.BumperType == bumperType);
        }
        #endregion
    }
}

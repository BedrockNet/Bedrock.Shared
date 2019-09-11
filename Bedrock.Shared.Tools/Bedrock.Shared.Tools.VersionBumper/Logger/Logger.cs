using System;

namespace Bedrock.Shared.Tools.VersionBumper.Logger
{
    public class Logger : ILogger
    {
        #region Constructors
        public Logger(Configuration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region Protected Properties
        protected Configuration Configuration { get; private set; }
        #endregion

        #region ILogger Methods
        public void Log(string message)
        {
            if (!Configuration.IsQuiet)
                Console.WriteLine(message);
        }
        #endregion
    }
}

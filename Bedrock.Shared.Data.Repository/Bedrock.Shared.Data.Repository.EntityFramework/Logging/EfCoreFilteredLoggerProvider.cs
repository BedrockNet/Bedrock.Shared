using Bedrock.Shared.Configuration;
using Bedrock.Shared.Log;

using Microsoft.Extensions.Logging;
using SharedInterface = Bedrock.Shared.Log.Interface;

namespace Bedrock.Shared.Data.Repository.EntityFramework.Logging
{
    public class EfCoreFilteredLoggerProvider : ILoggerProvider
    {
        #region Fields
        private readonly SharedInterface.ILogger _internalLogger;
        #endregion

        #region Constructors
        public EfCoreFilteredLoggerProvider(BedrockConfiguration bedrockConfiguration, SharedInterface.ILogger internalLogger = null)
        {
            BedrockConfiguration = bedrockConfiguration;
            _internalLogger = internalLogger;
        }
        #endregion

        #region Properties
        protected BedrockConfiguration BedrockConfiguration { get; set; }
        #endregion

        #region ILoggerProvider Members
        public ILogger CreateLogger(string categoryName)
        {
            var isFiltered = BedrockConfiguration.Log.Orm.IsFiltered;
            var categoryFilters = BedrockConfiguration.Log.Orm.CategoryFilters;

            if (!isFiltered || categoryFilters.Contains(categoryName))
                return new EfCoreFilteredLogger(BedrockConfiguration, _internalLogger);

            return new NullLogger();
        }
        #endregion

        #region IDisposable Members
        public void Dispose() { }
        #endregion
    }
}

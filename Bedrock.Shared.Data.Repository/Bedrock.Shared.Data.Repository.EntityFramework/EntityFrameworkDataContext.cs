using System;
using System.Linq;
using System.Threading.Tasks;

using Bedrock.Shared.Configuration;
using Bedrock.Shared.Data.Repository.EntityFramework.Logging;
using Bedrock.Shared.Service.Interface;

using CommonServiceLocator;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

using SharedInterface = Bedrock.Shared.Log.Interface;

namespace Bedrock.Shared.Data.Repository.EntityFramework
{
    public class EntityFrameworkDataContext : DbContext
    {
        #region Constructors
        public EntityFrameworkDataContext(BedrockConfiguration bedrockConfiguration)
        {
            BedrockConfiguration = bedrockConfiguration;
            Initialize();
        }

        public EntityFrameworkDataContext(string connectionString, BedrockConfiguration bedrockConfiguration)
        {
            ConnectionString = connectionString;
            BedrockConfiguration = bedrockConfiguration;
            Initialize();
        }

        public EntityFrameworkDataContext(DbContextOptions options, BedrockConfiguration bedrockConfiguration) : base(options)
        {
            IsInMemoryOptionsExtension = options.Extensions.FirstOrDefault(e => e.GetType().Name == "InMemoryOptionsExtension") != null;
            BedrockConfiguration = bedrockConfiguration;
            Initialize();
        }
        #endregion

        #region Properties
        protected string ConnectionString { get; set; }

        protected BedrockConfiguration BedrockConfiguration { get; set; }

        protected bool IsInMemoryOptionsExtension { get; set; }
        #endregion

        #region IUnitOfWork Methods
        public virtual int SaveChanges<T, C>(IServiceResponse<T, C> response)
            where T : class
            where C : class
        {
            throw new NotImplementedException();
        }

        public virtual Task<int> SaveChangesAsync<T, C>(IServiceResponse<T, C> response)
            where T : class
            where C : class
        {
            throw new NotImplementedException();
        }
        #endregion

        #region DbContext Methods
        // WARNING:  OnConfiguring occurs last and can overwrite options obtained from DI or the constructor. This approach does not lend itself to testing (unless you target the full database).
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrWhiteSpace(ConnectionString))
                optionsBuilder
                    .UseSqlServer(ConnectionString, b =>
                    {
                        if (BedrockConfiguration.Data.IsUseRowNumberForPaging)
                            b.UseRowNumberForPaging();
                    });

            if(BedrockConfiguration.Log.Orm.IsEnableSensitiveDataLogging)
                optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
        #endregion

        #region Protected Methods
        protected virtual void Initialize()
        {
            Database.AutoTransactionsEnabled = BedrockConfiguration.Data.IsAutoTransactionsEnabled;
            ChangeTracker.AutoDetectChangesEnabled = BedrockConfiguration.Data.AutoDetectChangesEnabled;

            if(!IsInMemoryOptionsExtension)
                Database.SetCommandTimeout(BedrockConfiguration.Data.CommandTimeout);

            if (BedrockConfiguration.Data.IsLog)
            {
                var loggerFactory = this.GetService<ILoggerFactory>();
                var internalLogger = BedrockConfiguration.Log.Orm.IsLog ? ServiceLocator.Current.GetInstance<SharedInterface.ILogger>() : default(SharedInterface.ILogger);

                loggerFactory.AddProvider(new EfCoreFilteredLoggerProvider(BedrockConfiguration, internalLogger));
            }
        }
        #endregion
    }
}

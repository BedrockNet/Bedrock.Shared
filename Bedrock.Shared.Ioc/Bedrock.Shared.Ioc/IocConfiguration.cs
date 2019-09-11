using Bedrock.Shared.Utility;

namespace Bedrock.Shared.Ioc
{
	public class IocConfiguration : Singleton<IocConfiguration>
	{
		#region Constructors
		public IocConfiguration()
		{
			IsRegisterLogger = true;

			IsRegisterMapper = true;

			IsRegisterMapperHelper = true;

			IsRegisterSession = true;

			IsRegisterRepositories = true;

			IsRegisterRepositoriesOrmId = true;

			IsRegisterOrmHelper = true;

			IsRegisterCacheProvider = true;

			IsRegisterQueueProvider = true;

			IsRegisterNoSqlProvider = true;

			IsRegisterSerializationProvider = true;

			IsRegisterHasher = true;

			IsRegisterServices = true;

			IsRegisterValidationConfigurators = true;

			IsRegisterEventDispatcher = true;

			IsRegisterDomainEventHandlers = true;

            IsRegisterWebApiClient = true;

            IsRegisterResourceAuthorizationManager = true;

            IsRegisterBedrockConfiguration = true;
		}
		#endregion

		#region Public Properties
		public bool IsRegisterLogger { get; set; }

		public bool IsRegisterMapper { get; set; }

		public bool IsRegisterMapperHelper { get; set; }

		public bool IsRegisterSession { get; set; }

		public bool IsRegisterRepositories { get; set; }

		public bool IsRegisterRepositoriesOrmId { get; set; }

		public bool IsRegisterOrmHelper { get; set; }

		public bool IsRegisterCacheProvider { get; set; }

		public bool IsRegisterQueueProvider { get; set; }

		public bool IsRegisterNoSqlProvider { get; set; }

		public bool IsRegisterSerializationProvider { get; set; }

		public bool IsRegisterHasher { get; set; }

		public bool IsRegisterServices { get; set; }

		public bool IsRegisterValidationConfigurators { get; set; }

		public bool IsRegisterEventDispatcher { get; set; }

		public bool IsRegisterDomainEventHandlers { get; set; }

        public bool IsRegisterWebApiClient { get; set; }

        public bool IsRegisterResourceAuthorizationManager { get; set; }

        public bool IsRegisterBedrockConfiguration { get; set; }
		#endregion
	}
}

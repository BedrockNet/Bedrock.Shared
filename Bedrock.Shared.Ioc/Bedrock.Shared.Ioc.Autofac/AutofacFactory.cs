using System;
using System.Reflection;

using Bedrock.Shared.Enumeration;
using Bedrock.Shared.Mapper.AutoMapper;

using Autofac;
using CommonServiceLocator;

using CoreConfiguration = Bedrock.Shared.Configuration;

namespace Bedrock.Shared.Ioc.Autofac
{
	public class AutofacFactory
	{
		#region Public Methods
		public static ContainerBuilder CreateContainerBuilder(Assembly bootstrapAssembly = null)
		{
			var returnValue = new ContainerBuilder();

			returnValue.RegisterModule(new SharedModule());
			returnValue.RegisterAssemblyModules(bootstrapAssembly);

			return returnValue;
		}

		/// <summary>
		/// Registers common application dependencies
		/// </summary>
		/// <param name="bootstrapAssembly">Used to scan for types</param>
		/// <param name="serviceAssemblies">Used to scan for types</param>
		/// <param name="domainAssemblies">Used to scan for types</param>
		/// <param name="dependentType">Type of application</param>
		/// <param name="bedrockConfiguration">If NetCoreApp application, must be passed in, or registration will throw.  If Framework application, will be built here, so no need to pass in.</param>
		/// <returns>Current builder</returns>
		public static ContainerBuilder RegisterAll
		(
			Assembly bootstrapAssembly = null,
			Assembly[] serviceAssemblies = null,
			Assembly[] domainAssemblies = null,
			DependentType dependentType = DependentType.Default,
			CoreConfiguration.BedrockConfiguration bedrockConfiguration = null,
			IocConfiguration iocConfiguration = null
		)
		{
			return RegisterAll(CreateContainerBuilder(bootstrapAssembly), bootstrapAssembly, serviceAssemblies, domainAssemblies, dependentType, bedrockConfiguration, iocConfiguration);
		}

		/// <summary>
		/// Registers common application dependencies
		/// </summary>
		/// <param name="builder">Current builder</param>
		/// <param name="bootstrapAssembly">Used to scan for types</param>
		/// <param name="serviceAssemblies">Used to scan for types</param>
		/// <param name="domainAssemblies">Used to scan for types</param>
		/// <param name="dependentType">Type of application</param>
		/// <param name="bedrockConfiguration">If NetCoreApp application, must be passed in, or registration will throw.  If Framework application, will be built here, so no need to pass in.</param>
		/// <returns>Current builder</returns>
		public static ContainerBuilder RegisterAll
		(
			ContainerBuilder builder,
			Assembly bootstrapAssembly = null,
			Assembly[] serviceAssemblies = null,
			Assembly[] domainAssemblies = null,
			DependentType dependentType = DependentType.Default,
			CoreConfiguration.BedrockConfiguration bedrockConfiguration = null,
			IocConfiguration iocConfiguration = null
		)
		{
			if (builder == null)
				throw new ArgumentNullException(nameof(builder));

			if ((dependentType == DependentType.AspNetCore || dependentType == DependentType.NetCoreApp) && bedrockConfiguration == null)
				throw new ArgumentNullException(nameof(bedrockConfiguration));

			var thisAssembly = Assembly.GetAssembly(typeof(AutofacFactory));
			var autoMapperAssembly = Assembly.GetAssembly(typeof(AutoMapperConfiguration));

			if (iocConfiguration == null)
				iocConfiguration = IocConfiguration.Current;

			return builder
					.RegisterLoggerNLog(iocConfiguration)
					.RegisterMapperAutoMapper(iocConfiguration, bootstrapAssembly, thisAssembly, autoMapperAssembly)
					.RegisterMapperHelper(iocConfiguration)
					.RegisterSession(dependentType, iocConfiguration)
					.RegisterOrmHelper(iocConfiguration)
					.RegisterCacheProvider(bedrockConfiguration, iocConfiguration)
					.RegisterQueueProvider(bedrockConfiguration, iocConfiguration)
					.RegisterSerializationProvider(iocConfiguration)
					.RegisterHasher(iocConfiguration)
					.RegisterServices(serviceAssemblies, iocConfiguration)
					.RegisterValidationConfigurators(domainAssemblies, iocConfiguration)
					.RegisterEventDispatcher(dependentType, iocConfiguration)
					.RegisterDomainEventHandlers(domainAssemblies, iocConfiguration)
					.RegisterWebApiClient(iocConfiguration);
		}

		public static void SetServiceLocator(IContainer container)
		{
			var serviceLocator = new AutofacServiceLocator(container);
			ServiceLocator.SetLocatorProvider(() => serviceLocator);
		}
		#endregion
	}
}

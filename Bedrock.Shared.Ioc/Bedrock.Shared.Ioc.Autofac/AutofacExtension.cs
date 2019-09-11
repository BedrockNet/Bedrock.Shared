using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac;
using Autofac.Core;

using Bedrock.Shared.Cache;
using Bedrock.Shared.Cache.Interface;
using Bedrock.Shared.Cache.Memory;
using Bedrock.Shared.Cache.Memory.Interface;
using Bedrock.Shared.Cache.Redis;
using Bedrock.Shared.Cache.Redis.Interface;

using Bedrock.Shared.Configuration;

using Bedrock.Shared.Data.Repository.EntityFramework;
using Bedrock.Shared.Data.Repository.EntityFramework.Helper;
using Bedrock.Shared.Data.Repository.Interface;

using Bedrock.Shared.Data.Validation.Interface;

using Bedrock.Shared.Domain.Implementation;
using Bedrock.Shared.Domain.Interface;

using Bedrock.Shared.Enumeration;
using Bedrock.Shared.Extension;

using Bedrock.Shared.Hash.BCrypt;
using Bedrock.Shared.Hash.Interface;

using Bedrock.Shared.Log.Interface;
using Bedrock.Shared.Log.NLog;

using Bedrock.Shared.Mapper.AutoMapper.Interface;
using Bedrock.Shared.Mapper.Interface;

using Bedrock.Shared.Queue;
using Bedrock.Shared.Queue.Interface;

using Bedrock.Shared.Security.Interface;
using Bedrock.Shared.Security.ResourceAuthorization;

using Bedrock.Shared.Serialization;
using Bedrock.Shared.Serialization.Binary;
using Bedrock.Shared.Serialization.Binary.Interface;
using Bedrock.Shared.Serialization.Interface;
using Bedrock.Shared.Serialization.Newtonsoft;
using Bedrock.Shared.Serialization.Newtonsoft.Interface;
using Bedrock.Shared.Serialization.Xml;
using Bedrock.Shared.Serialization.Xml.Interface;

using Bedrock.Shared.Service.Interface;

using Bedrock.Shared.Session.Implementation;
using Bedrock.Shared.Session.Interface;

using Bedrock.Shared.Web.Client.Interface;
using Bedrock.Shared.Web.Client;

using Autofac.Builder;
using Microsoft.EntityFrameworkCore;

using MapperAutoMapper = Bedrock.Shared.Mapper.AutoMapper;
using SystemReflection = System.Reflection;

namespace Bedrock.Shared.Ioc.Autofac
{
    public static class AutofacExtension
    {
        #region Public Methods
        public static T[] ResolveAll<T>(this IContainer self)
        {
            return self.Resolve<IEnumerable<T>>().ToArray();
        }

        public static object[] ResolveAll(this IContainer self, Type type)
        {
            var enumerableOfType = typeof(IEnumerable<>).MakeGenericType(type);
            return (object[])self.ResolveService(new TypedService(enumerableOfType));
        }

        public static ContainerBuilder RegisterLoggerNLog(this ContainerBuilder builder, IocConfiguration iocConfiguration)
        {
            if (!iocConfiguration.IsRegisterLogger)
                return builder; 

            builder
                .RegisterType<Logger>()
                .As<ILogger>()
                .SingleInstance();

            return builder;
        }

        public static ContainerBuilder RegisterMapperAutoMapper(this ContainerBuilder builder, IocConfiguration iocConfiguration, params SystemReflection.Assembly[] assemblies)
        {
            if (!iocConfiguration.IsRegisterMapper)
                return builder;

            MapperAutoMapper.AutoMapperConfiguration.Configure(assemblies);

            builder
                .RegisterType<MapperAutoMapper.MapperAutoMapper>()
                .As<IMapperAutoMapper>()
                .UsingConstructor()
                .SingleInstance();

            builder
                .RegisterType<MapperAutoMapper.Mapper>()
                .As<IMapper>()
                .UsingConstructor(typeof(IMapperAutoMapper))
                .SingleInstance();

            return builder;
        }

        public static ContainerBuilder RegisterMapperHelper(this ContainerBuilder builder, IocConfiguration iocConfiguration)
        {
            if (!iocConfiguration.IsRegisterMapperHelper)
                return builder;

            builder
                .RegisterType<MapperAutoMapper.MapperHelper>()
                .As<IMapperHelper>()
                .SingleInstance();

            return builder;
        }

        public static ContainerBuilder RegisterSession(this ContainerBuilder builder, DependentType dependentType, IocConfiguration iocConfiguration)
        {
            if (!iocConfiguration.IsRegisterSession)
                return builder;

            var registration = builder
								.RegisterType<BedrockSession>()
								.As<ISession>();

			return builder;
        }

		public static ContainerBuilder RegisterRepositoriesOrm(this ContainerBuilder builder, SystemReflection.Assembly assembly, IocConfiguration iocConfiguration, params Type[] includedTypes)
        {
            if (!iocConfiguration.IsRegisterRepositories)
                return builder;

            assembly
                .GetTypes()
                .Where(t => typeof(DbContext).IsAssignableFrom(t))
                .Each(t => t.GetProperties()
                            .Where(p => p.PropertyType.GetTypeInfo().IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                            .Select(p => p.PropertyType.GetGenericArguments().Single())
                            .Where(p => includedTypes.Contains(p))
                            .Each(r =>
                            {
                                var @interface = typeof(IRepositoryOrm<>).MakeGenericType(r);
                                var type = typeof(EntityFrameworkRepository<,>).MakeGenericType(r, t);

                                builder
                                .RegisterType(type)
                                .As(@interface);
                            }));

            return builder;
        }

        public static ContainerBuilder RegisterRepositoriesOrmId(this ContainerBuilder builder, SystemReflection.Assembly assembly, Dictionary<Type, Type> keyTypes, Type keyTypeDefault, IocConfiguration iocConfiguration, params Type[] includedTypes)
        {
            if (!iocConfiguration.IsRegisterRepositoriesOrmId)
                return builder;

            assembly
                .GetTypes()
                .Where(t => typeof(DbContext).GetTypeInfo().IsAssignableFrom(t))
                .Each(t =>
                    t.GetProperties()
                        .Where(p => p.PropertyType.GetTypeInfo().IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                        .Select(p => p.PropertyType.GetGenericArguments().Single())
                        .Where(p => includedTypes.Contains(p))
                        .Each(r =>
                        {
                            var keyType = GetKeyType(r, keyTypes, keyTypeDefault);
                            var @interface = typeof(IRepositoryOrmId<,>).MakeGenericType(r, keyType);
                            var type = typeof(EntityFrameworkIdRepository<,,>).MakeGenericType(r, t, keyType);

                            builder
                                .RegisterType(type)
                                .As(@interface);
                        }));

            return builder;
        }

        public static ContainerBuilder RegisterOrmHelper(this ContainerBuilder builder, IocConfiguration iocConfiguration)
        {
            if (!iocConfiguration.IsRegisterOrmHelper)
                return builder;

            builder
                .RegisterType<OrmHelper>()
                .As<IOrmHelper>()
                .SingleInstance();

            return builder;
        }

        public static ContainerBuilder RegisterCacheProvider(this ContainerBuilder builder, BedrockConfiguration bedrockConfiguration, IocConfiguration iocConfiguration)
        {
            if (!iocConfiguration.IsRegisterCacheProvider)
                return builder;

            var configuredCaches = bedrockConfiguration.Cache.ConfiguredCaches;

            if (configuredCaches.Contains(CacheType.Memory))
            {
                builder
                .RegisterType<MemoryCache>()
                .As<ICache>()
				.As<ICacheMemory>()
				.Named<ICache>("Memory")
                .SingleInstance();
            }

            if (configuredCaches.Contains(CacheType.Redis))
			{
				builder
				.RegisterType<RedisCache>()
				.As<ICache>()
				.As<ICacheRedis>()
				.Named<ICache>("Redis")
				.SingleInstance();
			}

			builder
                .RegisterType<CacheProvider>()
                .As<ICacheProvider>()
                .UsingConstructor(typeof(ICache[]), typeof(BedrockConfiguration))
                .SingleInstance();

            return builder;
        }

        public static ContainerBuilder RegisterQueueProvider(this ContainerBuilder builder, BedrockConfiguration bedrockConfiguration, IocConfiguration iocConfiguration)
        {
            if (!iocConfiguration.IsRegisterQueueProvider)
                return builder;

            var configuredQueues = bedrockConfiguration.Queue.ConfiguredQueues;

            if (configuredQueues.Contains(QueueType.Object))
            {
                builder
                .RegisterType<QueueObject>()
                .As<IQueue>()
                .As<IQueueObject>()
                .Named<IQueueObject>("Object");
            }

            builder
                .RegisterType<QueueProvider>()
                .As<IQueueProvider>()
                .UsingConstructor(typeof(IQueue[]), typeof(BedrockConfiguration));

            return builder;
        }

        public static ContainerBuilder RegisterSerializationProvider(this ContainerBuilder builder, IocConfiguration iocConfiguration)
        {
            if (!iocConfiguration.IsRegisterSerializationProvider)
                return builder;

            builder
                .RegisterType<SerializerXml>()
                .As<ISerializer>()
                .As<ISerializerXml>()
                .Named<ISerializer>("Xml")
                .SingleInstance();

            builder
                .RegisterType<SerializerBinary>()
                .As<ISerializer>()
                .As<ISerializerBinary>()
                .Named<ISerializer>("Binary")
                .SingleInstance();

            builder
                .RegisterType<SerializerNewtonsoft>()
                .As<ISerializer>()
                .As<ISerializerNewtonsoft>()
                .Named<ISerializer>("Newtonsoft")
                .SingleInstance();

            builder
                .RegisterType<SerializationProvider>()
                .As<ISerializationProvider>()
                .UsingConstructor(typeof(ISerializer[]))
                .SingleInstance();

            return builder;
        }

        public static ContainerBuilder RegisterHasher(this ContainerBuilder builder, IocConfiguration iocConfiguration)
        {
            if (!iocConfiguration.IsRegisterHasher)
                return builder;

            builder
                .RegisterType<Hasher>()
                .As<IHasher>()
                .SingleInstance();

            return builder;
        }

        public static ContainerBuilder RegisterServices(this ContainerBuilder builder, SystemReflection.Assembly[] serviceAssemblies, IocConfiguration iocConfiguration)
        {
            if (!iocConfiguration.IsRegisterServices)
                return builder;

            if (serviceAssemblies == null || !serviceAssemblies.Any())
                return builder;

            serviceAssemblies.Each(sa =>
            {
                sa
                .GetTypes()
                .Where(t => typeof(IService).GetTypeInfo().IsAssignableFrom(t))
                .Each(st =>
                {
                    var @interface = st
                                    .GetInterfaces()
                                    .FirstOrDefault(i => i.Name.EndsWith("Service")
                                                            && i.Name != nameof(IService));
                    if(@interface != null)
                        builder
                            .RegisterType(st)
                            .As(@interface)
                            .PropertiesAutowired();
                });   
            });

            return builder;
        }

        public static ContainerBuilder RegisterValidationConfigurators(this ContainerBuilder builder, SystemReflection.Assembly[] domainAssemblies, IocConfiguration iocConfiguration)
        {
            if (!iocConfiguration.IsRegisterValidationConfigurators)
                return builder;

            if (domainAssemblies == null || !domainAssemblies.Any())
                return builder;

            domainAssemblies.Each(da =>
            {
                da
                .GetTypes()
                .Where(t => typeof(IValidationConfigurator).GetTypeInfo().IsAssignableFrom(t))
                .Each(t => builder.RegisterType(t));
            });

            return builder;
        }

		public static ContainerBuilder RegisterEventDispatcher(this ContainerBuilder builder, DependentType dependentType, IocConfiguration iocConfiguration)
		{
            if (!iocConfiguration.IsRegisterEventDispatcher)
                return builder;

            var registration = builder
								.RegisterType<DomainEventDispatcher>()
								.As<IDomainEventDispatcher>();

			return builder;
		}

		public static ContainerBuilder RegisterDomainEventHandlers(this ContainerBuilder builder, SystemReflection.Assembly[] domainAssemblies, IocConfiguration iocConfiguration)
		{
            if (!iocConfiguration.IsRegisterDomainEventHandlers)
                return builder;

            if (domainAssemblies == null || !domainAssemblies.Any())
                return builder;

            domainAssemblies.Each(da =>
            {
                builder
                .RegisterAssemblyTypes(da)
                .AsClosedTypesOf(typeof(IDomainEventHandler<>));
            });

			return builder;
		}

		public static ContainerBuilder RegisterWebApiClient(this ContainerBuilder builder, IocConfiguration iocConfiguration)
		{
            if (!iocConfiguration.IsRegisterWebApiClient)
                return builder;

            builder
				.RegisterType<WebApiClient>()
				.As<IWebApiClient>()
				.SingleInstance();

			return builder;
		}

        public static ContainerBuilder RegisterResourceAuthorizationManager(this ContainerBuilder builder, IocConfiguration iocConfiguration)
        {
            if (!iocConfiguration.IsRegisterResourceAuthorizationManager)
                return builder;

            builder
               .RegisterType<BedrockResourceAuthorizationManager>()
               .As<IResourceAuthorizationManager>()
               .SingleInstance();

            return builder;
        }

		public static void SetWebRequestLifeTimeScope(DependentType dependentType, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration)
        {
            switch (dependentType)
            {
                case DependentType.AspNetCore:
                    {
                        registration.InstancePerLifetimeScope();
                        break;
                    }
                case DependentType.Web:
                case DependentType.WebApi:
                case DependentType.WebUI:
                    {
                        registration.InstancePerRequest();
                        break;
                    }
                default:
                    {
                        registration.SingleInstance();
                        break;
                    }
            }
        }
        #endregion

        #region Private Methods
        private static Type GetKeyType(Type type, Dictionary<Type, Type> keyTypes, Type keyTypeDefault)
        {
            var keyTypeGeneric = type.GetTypeInfo().BaseType.GetGenericArguments().Skip(1).FirstOrDefault();
            var keyType = (keyTypes != null && keyTypes.ContainsKey(type)) ? keyTypes[type] : keyTypeGeneric;

            return keyType != null ? keyType : keyTypeDefault;
        }
        #endregion
    }
}

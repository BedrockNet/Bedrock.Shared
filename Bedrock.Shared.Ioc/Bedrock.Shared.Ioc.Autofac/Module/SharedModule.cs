using Autofac;

using Bedrock.Shared.Cache.Interface;
using Bedrock.Shared.Log.Interface;
using Bedrock.Shared.Mapper.Interface;

namespace Bedrock.Shared.Ioc.Autofac
{
    public class SharedModule : Module
    {
        #region IModule Members
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ILogger>().PropertiesAutowired();
            builder.RegisterType<IMapper>().PropertiesAutowired();
            builder.RegisterType<ICacheProvider>().PropertiesAutowired();
        }
        #endregion
    }
}

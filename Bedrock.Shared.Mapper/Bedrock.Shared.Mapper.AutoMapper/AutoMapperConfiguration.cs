using System.Linq;
using System.Reflection;

using Bedrock.Shared.Extension;
using AM = AutoMapper;

namespace Bedrock.Shared.Mapper.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        #region Public Properties
        public static AM.IMapperConfigurationExpression MapperConfigurationExpression { get; private set; }
        #endregion

        #region Public Methods
        public static void Configure(params Assembly[] assembliesToScan)
        {
            assembliesToScan = assembliesToScan ?? new Assembly[] { };

            var allTypes = assembliesToScan
                            .SelectMany(a => a.ExportedTypes)
                            .ToArray();

            var profiles = allTypes
                            .Where(t => typeof(AM.Profile).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()))
                            .Where(t => !t.GetTypeInfo().IsAbstract);

            AM.Mapper.Initialize(c =>
            {
                MapperConfigurationExpression = c;
                profiles.Each(p => c.AddProfile(p));
            });
        }
        #endregion
    }
}

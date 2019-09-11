using Microsoft.AspNetCore.Mvc.ApiExplorer;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Bedrock.Shared.Web.Api.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        #region Fields
        private readonly IApiVersionDescriptionProvider provider;

        private readonly Info info;
        #endregion

        #region Constructors
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, Info info) => (this.provider, this.info) = (provider, info);
        #endregion

        #region IConfigureOptions Methods
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, UpdateInfoForApiVersion(info, description));
            }
        }
        #endregion

        #region Private Methods
        private static Info UpdateInfoForApiVersion(Info info, ApiVersionDescription description)
        {
            info.Version = description.ApiVersion.ToString();

            if (description.IsDeprecated)
                info.Description += " This API version has been deprecated.";

            return info;
        }
        #endregion
    }
}

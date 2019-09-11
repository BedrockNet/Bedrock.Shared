using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Bedrock.Shared.Configuration;
using Bedrock.Shared.Extension;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Bedrock.Shared.Web.Test.Helper
{
	public class Startup
	{
		#region Constructors
		public Startup(IHostingEnvironment env)
		{
			env.EnvironmentName = "Test";

			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddJsonFile("bedrockconfig.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"bedrockconfig.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();

			Configuration = builder.Build();
		}
		#endregion

		#region Public Properties
		public IConfigurationRoot Configuration { get; private set; }

		public static BedrockConfiguration BedrockConfiguration { get; private set; }
		#endregion

		#region Startup Methods
		public void ConfigureServices(IServiceCollection services)
		{
			BedrockConfiguration = services.ConfigurePoco<BedrockConfiguration>(Configuration.GetSection("BedrockConfig"));

            services
				.AddMvc()
                .AddApplicationPart(System.Reflection.Assembly.GetExecutingAssembly())
                .AddJsonOptions(options => {
					options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
					options.SerializerSettings.ContractResolver = new DefaultContractResolver();
				});
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
		#endregion
	}
}
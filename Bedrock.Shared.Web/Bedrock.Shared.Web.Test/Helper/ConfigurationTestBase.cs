using System.Net.Http;

using Bedrock.Shared.Configuration;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

using NUnit.Framework;

namespace Bedrock.Shared.Web.Test.Helper
{
	[SetUpFixture]
	public abstract class ConfigurationTestBase
    {
		#region Protected Properties
		public TestServer TestServer { get; set; }

		public HttpClient TestClient { get; set; }

		public BedrockConfiguration BedrockConfiguration { get; set; }
		#endregion

		#region SetUpFixture Members
		[OneTimeSetUp]
		public void GlobalSetup()
		{
			if (BedrockConfiguration != null)
				return;

			var builder = new WebHostBuilder()
								.UseStartup<Startup>();

			TestServer = new TestServer(builder);
			TestClient = TestServer.CreateClient();

			BedrockConfiguration = Startup.BedrockConfiguration;
		}

		[OneTimeTearDown]
		public void GlobalTeardown() { }
		#endregion
	}
}

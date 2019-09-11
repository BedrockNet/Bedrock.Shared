using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Utility;

using Bedrock.Shared.Web.Client;
using Bedrock.Shared.Web.Client.Response;

using Bedrock.Shared.Web.Exception;

using Bedrock.Shared.Web.Test.Helper;
using Bedrock.Shared.Web.Test.Helper.Entity;

using NUnit.Framework;

// ** Passing existing HttpClient instance, because it is created uniquely for tests

namespace Bedrock.Shared.Web.Test.Test
{
	[TestFixture]
	public class WebApiClientTest : ConfigurationTestBase
	{
        #region Test Methods (Get)
        [Test]
        [Category("Unit_Web_WebApiClient")]
        public async Task TestGetString()
        {
			var client = new WebApiClient(TestClient); 
			var url = "/api/webapiclienttest/TestGetString";
			var response = await client.GetAsync<string>(url);

			Assert.AreEqual("It worked!", response.ResponseValue);
			TestHttpResults(response);
		}

		[Test]
		[Category("Unit_Web_WebApiClient")]
		public async Task TestGetObject()
		{
			var client = new WebApiClient(TestClient);
			var url = "/api/webapiclienttest/TestGetObject";
			var response = await client.GetAsync<WaterBottle>(url);

			Assert.AreEqual("Ozarka", response.ResponseValue.Brand);
			TestHttpResults(response);
		}
        #endregion

        #region Test Methods (Post)
        [Test]
        [Category("Unit_Web_WebApiClient")]
        public async Task TestPostNoInputNoOutput()
        {
            var client = new WebApiClient(TestClient);
            var url = "/api/webapiclienttest/TestPostNoInputNoOutput";
            var response = await client.PostAsync(url);

            Assert.IsNull(response.ResponseValue);
            TestHttpResults(response);
        }

        [Test]
        [Category("Unit_Web_WebApiClient")]
        public async Task TestPostInputNoOutput()
        {
            var client = new WebApiClient(TestClient);
            var url = "/api/webapiclienttest/TestPostInputNoOutput";
            var response = await client.PostAsync(url, WaterBottle.CreateInstance());

            Assert.IsNull(response.ResponseValue);
            TestHttpResults(response);
        }

		[Test]
		[Category("Unit_Web_WebApiClient")]
		#pragma warning disable 1998
		public async Task TestPostBadInputNoOutput()
		{
			var exception = Assert.ThrowsAsync<HttpException>(async () => 
			{
				var client = new WebApiClient(TestClient);
				var url = "/api/webapiclienttest/TestPostInputNoOutput";
				var response = await client.PostAsync(url, (WaterBottle)null);
			});

			Assert.AreEqual("Invalid Object", exception.Message);
		}
		#pragma warning restore 1998

		[Test]
		[Category("Unit_Web_WebApiClient")]
		public async Task TestPostInputWithOutput()
		{
			var client = new WebApiClient(TestClient);
			var url = "/api/webapiclienttest/TestPostInputWithOutput";
			var response = await client.PostAsync(url, WaterBottle.CreateInstance());

			Assert.AreEqual("Ozarka", response.ResponseValue.Brand);
			TestHttpResults(response);
		}

		[Test]
		[Category("Unit_Web_WebApiClient")]
		public async Task TestPostInputWithDifferentOutput()
		{
			var client = new WebApiClient(TestClient);
			var url = "/api/webapiclienttest/TestPostInputWithDifferentOutput";
			var response = await client.PostAsync<Speaker, WaterBottle>(url, WaterBottle.CreateInstance());

			Assert.AreEqual("JBL", response.ResponseValue.Brand);
			TestHttpResults(response);
		}
		#endregion

		#region Test Methods (Put)
		[Test]
		[Category("Unit_Web_WebApiClient")]
		public async Task TestPutInputWithOutput()
		{
			var client = new WebApiClient(TestClient);
			var url = "/api/webapiclienttest/TestPutInputWithOutput";
			var response = await client.PutAsync(url, WaterBottle.CreateInstance());

			Assert.AreEqual("Ozarka", response.ResponseValue.Brand);
			TestHttpResults(response);
		}

		[Test]
		[Category("Unit_Web_WebApiClient")]
		public async Task TestPutInputWithDifferentOutput()
		{
			var client = new WebApiClient(TestClient);
			var url = "/api/webapiclienttest/TestPutInputWithDifferentOutput";
			var response = await client.PutAsync<Speaker, WaterBottle>(url, WaterBottle.CreateInstance());

			Assert.AreEqual("JBL", response.ResponseValue.Brand);
			TestHttpResults(response);
		}
		#endregion

		#region Test Methods (Delete)
		[Test]
		[Category("Unit_Web_WebApiClient")]
		public async Task TestDelete()
		{
			var client = new WebApiClient(TestClient);
			var url = "/api/webapiclienttest/TestDelete";
			var response = await client.DeleteAsync(url);

			Assert.IsTrue(response.ResponseValue);
			TestHttpResults(response);
		}
        #endregion

        #region Test Methods (Configuration_Changed)
        [Test]
        [Category("Unit_Web_WebApiClient")]
        public async Task TestConfigurationChangedBearerTokenEmpty()
        {
            var client = new WebApiClient(TestClient);
            var url = "/api/webapiclienttest/TestGetString";

            client.Configuration.BearerToken = string.Empty;

            var response = await client.GetAsync<string>(url);

            Assert.AreEqual("It worked!", response.ResponseValue);
            TestHttpResults(response);
        }

        [Test]
        [Category("Unit_Web_WebApiClient")]
        public async Task TestConfigurationChangedBearer()
        {
            var client = new WebApiClient(TestClient);
            var url = "/api/webapiclienttest/TestGetString";

            client.Configuration.BearerToken = "I_Can_Get_In";

            var response = await client.GetAsync<string>(url);

            Assert.AreEqual("It worked!", response.ResponseValue);
            TestHttpResults(response);
        }

        [Test]
        [Category("Unit_Web_WebApiClient")]
        public async Task TestConfigurationChangedBaseAddressEmpty()
        {
            var client = new WebApiClient(TestClient);
            var url = "/api/webapiclienttest/TestGetString";

            client.Configuration.BaseAddress = string.Empty;

            var response = await client.GetAsync<string>(url);

            Assert.AreEqual("It worked!", response.ResponseValue);
            TestHttpResults(response);
        }

        [Test]
        [Category("Unit_Web_WebApiClient")]
        public async Task TestConfigurationChangedAcceptHeaders()
        {
            var client = new WebApiClient(TestClient);
            var url = "/api/webapiclienttest/TestGetString";

            client.Configuration.AcceptHeaders.Clear();
            client.Configuration.AcceptHeaders.Add(new MediaTypeWithQualityHeaderValue(StringHelper.Current.Lookup(StringMediaType.TextJson)));

            var response = await client.GetAsync<string>(url);

            Assert.AreEqual("It worked!", response.ResponseValue);
            TestHttpResults(response);
        }

        [Test]
        [Category("Unit_Web_WebApiClient")]
        public async Task TestConfigurationChangedAcceptHeaderEncodings()
        {
            var client = new WebApiClient(TestClient);
            var url = "/api/webapiclienttest/TestGetString";

            client.Configuration.AcceptHeaderEncodings.Clear();
            client.Configuration.AcceptHeaderEncodings.Add(new StringWithQualityHeaderValue("gzip"));

            var response = await client.GetAsync<string>(url);

            Assert.AreEqual("It worked!", response.ResponseValue);
            TestHttpResults(response);
        }
        #endregion

        #region Private Methods
        private void TestHttpResults<T>(WebApiResponse<T> response)
		{
			Assert.IsTrue(response.ResponseMessage.IsSuccessStatusCode);
			Assert.AreEqual("OK", response.ResponseMessage.ReasonPhrase);
			Assert.AreEqual(200, response.ResponseMessage.StatusCode);
			Assert.AreEqual(0, response.Errors.Count());
		}
		#endregion
	}
}

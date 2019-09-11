using System.Net;
using System.Threading.Tasks;

using Bedrock.Shared.Web.Api.Controller;
using Bedrock.Shared.Web.Exception;
using Bedrock.Shared.Web.Test.Helper.Entity;

using Microsoft.AspNetCore.Mvc;

namespace Bedrock.Shared.Web.Test.Controller
{
	[Produces("application/json")]
	[Route("api/webapiclienttest")]
	public class WebApiClientTestController : BedrockApiController
	{
		#region Api Methods (Get)
		[HttpGet, Route("TestGetString")]
		public async Task<IActionResult> TestGetString()
		{
			var returnValue = await Task.FromResult("It worked!");
			return Ok(returnValue);
		}

		[HttpGet, Route("TestGetObject")]
		public async Task<IActionResult> TestGetObject()
		{
			var returnValue = await Task.FromResult(WaterBottle.CreateInstance());
			return Ok(returnValue);
		}
		#endregion

		#region Api Methods (Post)
		[HttpPost, Route("TestPostNoInputNoOutput")]
		public async Task<IActionResult> TestPostNoInputNoOutput()
		{
			await Task.Delay(0);
			return Ok();
		}

		[HttpPost, Route("TestPostInputNoOutput")]
		public async Task<IActionResult> TestPostInputNoOutput([FromBody]WaterBottle waterBottle)
		{
			await Task.Delay(0);

			if (waterBottle != null && waterBottle.Brand == "Ozarka")
				return Ok();
			else
				throw new HttpException(HttpStatusCode.BadRequest, "Invalid Object");
		}

		[HttpPost, Route("TestPostInputWithOutput")]
		public async Task<IActionResult> TestPostInputWithOutput([FromBody]WaterBottle waterBottle)
		{
			await Task.Delay(0);

			if (waterBottle != null && waterBottle.Brand == "Ozarka")
				return Ok(waterBottle);
			else
				throw new HttpException(HttpStatusCode.BadRequest, "Invalid Object");
		}

		[HttpPost, Route("TestPostInputWithDifferentOutput")]
		public async Task<IActionResult> TestPostInputWithDifferentOutput([FromBody]WaterBottle waterBottle)
		{
			await Task.Delay(0);

			if (waterBottle != null && waterBottle.Brand == "Ozarka")
				return Ok(Speaker.CreateInstance());
			else
				throw new HttpException(HttpStatusCode.BadRequest, "Invalid Object");
		}
		#endregion

		#region Api Methods (Put)
		[HttpPut, Route("TestPutInputWithOutput")]
		public async Task<IActionResult> TestPutInputWithOutput([FromBody]WaterBottle waterBottle)
		{
			await Task.Delay(0);

			if (waterBottle != null && waterBottle.Brand == "Ozarka")
				return Ok(waterBottle);
			else
				throw new HttpException(HttpStatusCode.BadRequest, "Invalid Object");
		}

		[HttpPut, Route("TestPutInputWithDifferentOutput")]
		public async Task<IActionResult> TestPutInputWithDifferentOutput([FromBody]WaterBottle waterBottle)
		{
			await Task.Delay(0);

			if (waterBottle != null && waterBottle.Brand == "Ozarka")
				return Ok(Speaker.CreateInstance());
			else
				throw new HttpException(HttpStatusCode.BadRequest, "Invalid Object");
		}
		#endregion

		#region Api Methods (Delete)
		[HttpDelete, Route("TestDelete")]
		public async Task<IActionResult> TestDelete()
		{
			await Task.Delay(0);
			return Ok(true);
		}
		#endregion
	}
}

using System.Net.Http;
using System.Threading.Tasks;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Utility;

using Newtonsoft.Json;

namespace Bedrock.Shared.Web.Extension
{
	public static class HttpContentExtension
	{
		#region Private Properties
		private static string MediaTypeApplicationJson => StringHelper.Current.Lookup(StringMediaType.ApplicationJson);
		#endregion

		#region Public Methods
		public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
		{
            var returnValue = default(T);
			var dataAsString = await content.ReadAsStringAsync();

            if(!string.IsNullOrWhiteSpace(dataAsString))
                returnValue = JsonConvert.DeserializeObject<T>(dataAsString);

            return returnValue;
        }
		#endregion
	}
}

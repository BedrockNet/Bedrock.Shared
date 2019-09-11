using Bedrock.Shared.Enumeration;

namespace Bedrock.Shared.Configuration
{
	public class Application
	{
		#region Properties
		public string Name { get; set; }

		public Environment Environment { get; set; }

		public string SupportEmailAddress { get; set; }
		#endregion
	}
}

namespace Bedrock.Shared.Configuration
{
	public class BedrockConfiguration
	{
		#region Properties
		public Application Application { get; set; }

		public Cache Cache { get; set; }

		public Data Data { get; set; }

		public Email Email { get; set; }

		public Log Log { get; set; }

		public DistributedService DistributedService { get; set; }

		public Hash Hash { get; set; }

		public Queue Queue { get; set; }

        public Security Security { get; set; }
		#endregion
	}
}

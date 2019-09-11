using Bedrock.Shared.Enumeration;

namespace Bedrock.Shared.Configuration
{
	public class Hash
	{
		#region Properties
		public int WorkFactor { get; set; }

		public int ShiftFactor { get; set; }

		public int ShiftOffset { get; set; }

		public ShiftDirection ShiftDirection { get; set; }

		public bool IsKillSalt { get; set; }
		#endregion
	}
}

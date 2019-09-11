namespace Bedrock.Shared.Web.Test.Helper.Entity
{
    public class WaterBottle
    {
		#region Public Properties
		public decimal Capacity { get; set; }

		public string Brand { get; set; }
		#endregion

		#region Public Methods
		public static WaterBottle CreateInstance()
		{
			return new WaterBottle
			{
				Capacity = 2.25m,
				Brand = "Ozarka"
			};
		}
		#endregion
	}
}

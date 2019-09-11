namespace Bedrock.Shared.Web.Test.Helper.Entity
{
	public class Speaker
	{
		#region Public Properties
		public int Size { get; set; }

		public string Brand { get; set; }
		#endregion

		#region Public Methods
		public static Speaker CreateInstance()
		{
			return new Speaker
			{
				Size = 5,
				Brand = "JBL"
			};
		}
		#endregion
	}
}

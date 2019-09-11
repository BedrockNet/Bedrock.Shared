using Bedrock.Shared.Data.Repository.Enumeration;

namespace Bedrock.Shared.Data.Repository
{
	public class SqlParameter
	{
		#region Constructors
		public SqlParameter(string name, object value, SqlParameterDirection direction = SqlParameterDirection.Input)
		{
			Name = name;
			Value = value;
			Direction = direction;
		}
		#endregion

		#region Properties
		public string Name { get; set; }

		public object Value { get; set; }

		public SqlParameterDirection Direction { get; set; }
		#endregion

		#region Public Methods
		public static SqlParameter CreateInstance(string name, object value, SqlParameterDirection direction = SqlParameterDirection.Input)
		{
			return new SqlParameter(name, value, direction);
		}
		#endregion
	}
}

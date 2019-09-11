using System;
using System.Collections.Generic;
using System.Data;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Extension;
using Bedrock.Shared.Utility;

using DataSqlClinet = System.Data.SqlClient;

namespace Bedrock.Shared.Data.Repository.Extension
{
	public static class SqlParameterExtension
	{
		#region Public Methods
		public static Tuple<string, object[]> PrepareArguments(this SqlParameter[] parameters, string storedProcedure)
		{
			if (parameters == null)
				return new Tuple<string, object[]>(storedProcedure, new object[] { });

			var parameterNames = new List<string>();
			var parameterValues = new List<object>();

			parameters.Each(p =>
			{
				var name = string.Concat(StringHelper.Current.Lookup(StringCharacter.Ampersand), p.Name);
				parameterNames.Add(name);

				var parameter = new DataSqlClinet.SqlParameter(name, p.Value ?? DBNull.Value);
				parameter.Direction = (ParameterDirection)p.Direction;

				parameterValues.Add(parameter);
			});

			// Format: <stored_procedure_name> @param1 [, @param2] [, @param3]
			// Example: Get_Product @LocationId, @ProductId
			if (parameterNames.Count > 0)
				storedProcedure += " " + string.Join(", ", parameterNames);

			return new Tuple<string, object[]>(storedProcedure, parameterValues.ToArray());
		}
		#endregion
	}
}

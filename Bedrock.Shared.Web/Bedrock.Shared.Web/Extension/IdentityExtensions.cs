using System.Security.Principal;

namespace Bedrock.Shared.Web.Extension
{
	public static class IdentityExtensions
	{
		public static string GetLogin(this IIdentity identity)
		{
            var returnValue = default(string);

			var name = identity?.Name;
			var stop = name?.IndexOf("\\");

            if(name != null && stop.HasValue)
                returnValue = (stop > -1) ? name.Substring(stop.Value + 1, name.Length - stop.Value - 1) : string.Empty;

            return returnValue;
        }
	}
}

using Microsoft.AspNetCore.Hosting;
using Bedrock.Shared.Enumeration;

namespace Bedrock.Shared.Web.Extension
{
    public static class WebExtension
    {
        #region Public Methods
        public static bool IsLocal(this IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.EnvironmentName == Environment.Local.ToString();
        }
        #endregion
    }
}

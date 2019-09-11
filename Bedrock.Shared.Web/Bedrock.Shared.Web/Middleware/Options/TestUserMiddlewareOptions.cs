using System;
using System.Collections.Generic;
using System.Text;

namespace Bedrock.Shared.Web.Middleware.Options
{
    public class TestUserMiddlewareOptions
    {
        public TestUserMiddlewareOptions(string[] applications)
        {
            Applications = applications;
        }

        #region Properties
        public string[] Applications { get; set; }
        #endregion
    }
}

using System;
using System.Collections.Generic;

using Bedrock.Shared.Security.Interface;

namespace Bedrock.Shared.Security.ResourceAuthorization
{
    public class ResourceAuthorizationMiddlewareOptions
    {
        public ResourceAuthorizationMiddlewareOptions()
        {
            ManagerProvider = (items) => null;
        }

        #region Properties
        public IResourceAuthorizationManager Manager { get; set; }

        public Func<IDictionary<object, object>, IResourceAuthorizationManager> ManagerProvider { get; set; }
        #endregion
    }
}

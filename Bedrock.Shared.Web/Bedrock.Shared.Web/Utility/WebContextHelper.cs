using System;
using Bedrock.Shared.Web.Interface;
using Microsoft.AspNetCore.Http;

namespace Bedrock.Shared.Web.Utility
{
    public class WebContextHelper : IWebContextHelper
    {
        #region Constructors
        public WebContextHelper(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Properties
        protected IHttpContextAccessor HttpContextAccessor { get; set; }
        #endregion

        #region IWebContextHelper Methods
        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public object GetService(Type type)
        {
            return HttpContextAccessor.HttpContext.RequestServices.GetService(type);
        }
        #endregion
    }
}

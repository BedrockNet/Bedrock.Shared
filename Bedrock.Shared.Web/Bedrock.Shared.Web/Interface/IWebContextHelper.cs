using System;

namespace Bedrock.Shared.Web.Interface
{
    public interface IWebContextHelper
    {
        #region Methods
        T GetService<T>();

        object GetService(Type type);
        #endregion
    }
}

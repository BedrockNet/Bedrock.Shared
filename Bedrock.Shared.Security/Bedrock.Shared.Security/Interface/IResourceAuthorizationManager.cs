using System.Threading.Tasks;

namespace Bedrock.Shared.Security.Interface
{
    public interface IResourceAuthorizationManager
    {
        #region Methods
        Task<bool> CheckAccessAsync(IResourceAuthorizationContext context);
        #endregion
    }
}

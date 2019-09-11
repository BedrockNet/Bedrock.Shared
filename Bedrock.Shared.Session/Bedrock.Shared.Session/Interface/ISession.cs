using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Bedrock.Shared.Security.Interface;
using Bedrock.Shared.Service.Interface;

namespace Bedrock.Shared.Session.Interface
{
    public interface ISession : IDisposable
    {
        #region Properties
        IBedrockUser User { get; }

		ClaimsPrincipal Principal { get; }
		#endregion

		#region Methods
		void SetUser(IBedrockUser user);

        void SetUser(IBedrockUser user, ClaimsPrincipal principal);

        IUnitOfWork GetUnitOfWork<T>() where T : IUnitOfWork;

        void Complete();

        void Complete<T, C>(IServiceResponse<T, C> response)
            where T : class
            where C : class;

        Task<int>[] CompleteAsync();

        Task<int>[] CompleteAsync<T, C>(IServiceResponse<T, C> response)
            where T : class
            where C : class;

        void Reset();
        #endregion
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Service.Interface;

namespace Bedrock.Shared.Session.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        #region Properties
        ISession Session { get; set; }
        #endregion

        #region Methods
        int SaveChanges();

        int SaveChanges<T, C>(IServiceResponse<T, C> response)
            where T : class
            where C : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<int> SaveChangesAsync<T, C>(IServiceResponse<T, C> response)
            where T : class
            where C : class;
        #endregion
    }
}

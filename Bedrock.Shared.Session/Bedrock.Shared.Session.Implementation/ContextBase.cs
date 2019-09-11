using Bedrock.Shared.Session.Interface;

namespace Bedrock.Shared.Session.Implementation
{
    public abstract class ContextBase<TContext> : SessionAwareBase, ISessionAware
        where TContext : IUnitOfWork
    {
        #region Properties
        protected virtual TContext Context { get { return (TContext)Session.GetUnitOfWork<TContext>(); } }
        #endregion
    }
}

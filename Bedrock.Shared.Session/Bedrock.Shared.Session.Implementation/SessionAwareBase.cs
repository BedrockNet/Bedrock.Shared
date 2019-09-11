using System;

using Bedrock.Shared.Extension;
using Bedrock.Shared.Session.Interface;

namespace Bedrock.Shared.Session.Implementation
{
    public abstract class SessionAwareBase : ISessionAware
    {
        #region Fields
        private ISession _session;
        #endregion

        #region Constructors
        public SessionAwareBase(params ISessionAware[] sessionAwareDependencies)
        {
            SessionAwareDependencies = sessionAwareDependencies;
        }
        #endregion

        #region Properties
        protected ISessionAware[] SessionAwareDependencies { get; set; }
        #endregion

        #region ISessionAware Properties
        public virtual ISession Session
        {
            get
            {
                if (_session == null)
                    throw new InvalidOperationException("Session not enlisted");

                return _session;
            }

            set { _session = value; }
        }
        #endregion

        #region ISessionAware Methods
        public virtual void Enlist(ISession session)
        {
            Session = session;
            SessionAwareDependencies.Each(sad => sad.Enlist(session));
        }
        #endregion
    }
}

namespace Bedrock.Shared.Session.Interface
{
    public interface ISessionAware
    {
        #region Properties
        ISession Session { get; set; }
        #endregion

        #region Methods
        void Enlist(ISession session);
        #endregion
    }
}

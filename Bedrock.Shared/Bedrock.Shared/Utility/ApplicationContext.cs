namespace Bedrock.Shared.Utility
{
    public static class ApplicationContext
    {
        #region Fields
        private static DomainGraphCache _domainGraphCache;
        #endregion

        #region Properties
        public static DomainGraphCache DomainGraphCache
        {
            get
            {
                if (_domainGraphCache == null)
                    _domainGraphCache = new DomainGraphCache();

                return _domainGraphCache;
            }
        }
        #endregion
    }
}

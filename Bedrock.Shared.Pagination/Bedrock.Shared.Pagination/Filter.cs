using Bedrock.Shared.Pagination.Enumeration;

namespace Bedrock.Shared.Pagination
{
    public class Filter
    {
        #region Public Properties
        public string Key { get; set; }

        public string[] Values { get; set; }

        public FilterMatchMode MatchMode { get; set; }
        #endregion
    }
}

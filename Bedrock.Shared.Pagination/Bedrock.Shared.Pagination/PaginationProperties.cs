using System.Collections.Generic;
using Bedrock.Shared.Pagination.Enumeration;

namespace Bedrock.Shared.Pagination
{
    public class PaginationProperties
    {
        #region Public Properties
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string SortField { get; set; }

        public SortOrder SortOrder { get; set; }

        public IEnumerable<Filter> Filters { get; set; }
        #endregion
    }
}

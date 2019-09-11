using System;
using System.Collections.Generic;

namespace Bedrock.Shared.Pagination
{
    public class PaginationResult<T>
    {
        #region Constructors
        public PaginationResult() { }

        public PaginationResult(List<T> data, int pageIndex, int pageSize, int totalCount)
        {
            if(data == null)
                throw new ArgumentNullException(nameof(data));

            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            Data = data;
        }
        #endregion

        #region Public Methods
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPageCount
        {
            get { return (int)Math.Ceiling(TotalCount / (double)PageSize); }
        }

        public bool HasPreviousPage
        {
            get { return PageIndex > 1; }
        }

        public bool HasNextPage
        {
            get { return PageIndex < TotalPageCount; }
        }

        public List<T> Data { get; set; }
        #endregion
    }
}

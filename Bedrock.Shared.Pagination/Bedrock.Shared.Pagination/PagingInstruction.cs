using System.Collections.Generic;

namespace Bedrock.Shared.Pagination
{
    public sealed class PagingInstruction
    {
        #region Constructors
        public PagingInstruction()
        {
            SortOptions = new List<SortingInstruction>();
        }
        #endregion

        #region Properties
        public int PageIndex { get; set; }


        public int PageSize { get; set; }


        public List<SortingInstruction> SortOptions { get; set; }


        public int TotalRowCount { get; set; }
        #endregion
    }
}

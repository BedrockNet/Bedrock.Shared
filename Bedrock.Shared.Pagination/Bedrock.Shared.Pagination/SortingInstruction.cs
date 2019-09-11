using Bedrock.Shared.Pagination.Enumeration;

namespace Bedrock.Shared.Pagination
{
    public class SortingInstruction
    {
        #region Properties
        public string Column { get; set; }

        public SortOrder Direction { get; set; }
        #endregion
    }
}

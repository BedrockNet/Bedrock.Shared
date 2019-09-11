using System.ComponentModel;

namespace Bedrock.Shared.Pagination.Enumeration
{
    public enum CompareOperator
    {
        [Description("=")]
        Equal = 1,

        [Description("<")]
        LessThan = 2,

        [Description("<=")]
        LessThanOrEqual = 3,

        [Description(">")]
        GreaterThan = 4,

        [Description(">=")]
        GreaterThanOrEqual = 5
    }
}

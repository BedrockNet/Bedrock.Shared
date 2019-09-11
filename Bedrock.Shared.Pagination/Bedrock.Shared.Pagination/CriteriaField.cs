using System;
using Bedrock.Shared.Pagination.Enumeration;

namespace Bedrock.Shared.Pagination
{
    public abstract class CriteriaField
    {
        public CompareOperator Criteria { get; set; }
    }

    public class CriteriaFieldDateTime : CriteriaField
    {
        public DateTime Cvalue { get; set; }
    }

    public class CriteriaFieldNullableDateTime : CriteriaField
    {
        public DateTime? Cvalue { get; set; }
    }

    public class CriteriaFieldNullableDecimal : CriteriaField
    {
        public Decimal? Cvalue { get; set; }
    }
}

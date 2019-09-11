using System;

namespace Bedrock.Shared.Entity.Interface
{
    public interface IBedrockAuditEntity : IBedrockEntity
    {
        #region Properties
        int CreatedBy { get; set; }

        DateTime CreatedDate { get; set; }

        int? UpdatedBy { get; set; }

        DateTime? UpdatedDate { get; set; }
        #endregion
    }
}

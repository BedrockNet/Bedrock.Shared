using System;

namespace Bedrock.Shared.Entity.Interface
{
    public interface IBedrockDeletableEntity : IBedrockAuditEntity
    {
        #region Properties
        bool IsDeleted { get; set; }

        int? DeletedBy { get; set; }

        DateTime? DeletedDate { get; set; }
        #endregion
    }
}

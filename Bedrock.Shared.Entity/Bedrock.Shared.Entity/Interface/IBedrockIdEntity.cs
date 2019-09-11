using System;

namespace Bedrock.Shared.Entity.Interface
{
    public interface IBedrockIdEntity<TKey>
        where TKey : IComparable
    {
        #region Properties
        TKey Id { get; set; }
        #endregion
    }
}

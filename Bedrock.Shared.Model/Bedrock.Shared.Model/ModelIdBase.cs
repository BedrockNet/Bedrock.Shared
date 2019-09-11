using System;
using Bedrock.Shared.Entity.Interface;

namespace Bedrock.Shared.Model
{
    public abstract class ModelIdBase<K> : IBedrockIdEntity<K>
        where K : IComparable
    {
        #region IIdEntity Properties
        public K Id { get; set; }
        #endregion
    }
}

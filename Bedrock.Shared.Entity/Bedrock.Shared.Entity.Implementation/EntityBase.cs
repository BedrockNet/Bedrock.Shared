using System;

namespace Bedrock.Shared.Entity.Implementation
{
    [Serializable]
    public abstract class EntityBase<TEntity>
        where TEntity : EntityBase<TEntity>
    { }
}

using System;
using Bedrock.Shared.Entity.Interface;

namespace Bedrock.Shared.Entity.Implementation
{
    [Serializable]
    public abstract class EntityIdBase<TEntity, TKey> : IBedrockIdEntity<TKey>, IEquatable<TEntity>
        where TEntity : EntityIdBase<TEntity, TKey>
        where TKey : IComparable
    {
        #region Fields
        private TKey _id;
        #endregion

        #region Public Properties
        public TKey Id
        {
            get { return _id; }
            set { _id = value; }
        }
        #endregion

        #region IEquatable<TEntity>
        public bool Equals(TEntity other)
        {
            if (other == null)
                return false;

            return ReferenceEquals(other, this) || (other.Id.Equals(Id) && !Id.Equals(default(TKey)));
        }
        #endregion

        #region Public Methods
        public override bool Equals(object obj)
        {
            var entityBase = obj as TEntity;

            if (entityBase == null)
                return false;

            return ReferenceEquals(entityBase, this) || (entityBase.Id.Equals(Id) && !Id.Equals(default(TKey)));
        }

        public override int GetHashCode()
        {
            return Id.Equals(default(TKey)) ? base.GetHashCode() : Id.GetHashCode();
        }
        #endregion
    }
}

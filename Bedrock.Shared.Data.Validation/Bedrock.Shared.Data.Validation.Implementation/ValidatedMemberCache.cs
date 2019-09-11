using System.Collections.Concurrent;
using System.Linq;

using Bedrock.Shared.Data.Validation.Interface;

namespace Bedrock.Shared.Data.Validation.Implementation
{
    public class ValidatedMemberCache
    {
        #region Fields
        private ConcurrentBag<IValidatableEntity> _cachedMembers;
        #endregion

        #region Constructor
        private ValidatedMemberCache()
        {
            _cachedMembers = new ConcurrentBag<IValidatableEntity>();
        }
        #endregion

        #region Method
        public static ValidatedMemberCache CreateInstance()
        {
            return new ValidatedMemberCache();
        }

        public void AddValidatedMember(IValidatableEntity member)
        {
            if (!_cachedMembers.Contains(member))
                _cachedMembers.Add(member);
        }

        public bool Contains(IValidatableEntity member)
        {
            return _cachedMembers.Contains(member);
        }

        public void ClearCache()
        {
            IValidatableEntity validatableEntity;

            while (!_cachedMembers.IsEmpty)
                _cachedMembers.TryTake(out validatableEntity);
        }
        #endregion
    }
}

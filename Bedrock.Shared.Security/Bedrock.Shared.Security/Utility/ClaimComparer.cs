using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using Bedrock.Shared.Enumeration.StringHelper;
using Bedrock.Shared.Utility;

namespace Bedrock.Shared.Security.Utility
{
    public class ClaimComparer : IEqualityComparer<Claim>
    {
        #region IEqualityComparer Methods
        public bool Equals(Claim x, Claim y)
        {
            if (x == null && y == null)
                return true;

            if (x == null && y != null)
                return false;

            if (x != null && y == null)
                return false;

            var returnValue = x.Type == y.Type &&
                                x.Value == y.Value &&
                                x.ValueType == y.ValueType;

            var resourceTypeX = GetResourceType(x);
            var resourceTypeY = GetResourceType(y);

            if (!string.IsNullOrWhiteSpace(resourceTypeX) && !string.IsNullOrWhiteSpace(resourceTypeY))
                returnValue = returnValue && (resourceTypeX == resourceTypeY);

            return returnValue;
        }

        public int GetHashCode(Claim claim)
        {
            if (ReferenceEquals(claim, null))
                return default(int);

            int typeHash = claim.Type == null ? 0 : claim.Type.GetHashCode();
            int valueHash = claim.Value == null ? 0 : claim.Value.GetHashCode();
            int valueTypeHash = claim.ValueType == null ? 0 : claim.ValueType.GetHashCode();

            return typeHash ^ valueHash ^ valueTypeHash;
        }
        #endregion

        #region Private Properties
        private string GetResourceType(Claim claim)
        {
            var returnValue = default(string);
            var resourceTypeProperty = StringHelper.Current.Lookup(StringSecurity.ResourceType);

            if (claim.Properties != null && claim.Properties.Any() && claim.Properties.ContainsKey(resourceTypeProperty))
                returnValue = claim.Properties[resourceTypeProperty];

            return returnValue;
        }
        #endregion
    }
}

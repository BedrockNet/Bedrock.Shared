using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Bedrock.Shared.Utility
{
    public class ObjectVisitationHelper : IEqualityComparer<object>
    {
        #region Fields
        private readonly Dictionary<object, object> _objectSet;
        #endregion

        #region Constructors
        public ObjectVisitationHelper(object obj = null)
        {
            _objectSet = new Dictionary<object, object>(this);

            if (obj != null)
                _objectSet.Add(obj, obj);
        }

        public ObjectVisitationHelper(IEqualityComparer<object> comparer)
        {
            _objectSet = new Dictionary<object, object>(comparer);
        }

        private ObjectVisitationHelper(ObjectVisitationHelper other)
        {
            _objectSet = other._objectSet.ToDictionary
            (
                x => x.Key,
                y => y.Value,
                other._objectSet.Comparer
            );
        }
        #endregion

        #region Public Static Methods
        public static ObjectVisitationHelper CreateInstance(object obj = null)
        {
            return new ObjectVisitationHelper(obj);
        }

        public static ObjectVisitationHelper CreateInstance(IEqualityComparer<object> comparer)
        {
            return new ObjectVisitationHelper(comparer);
        }

        public static void EnsureCreated(ref ObjectVisitationHelper visitationHelper)
        {
            if (visitationHelper == null)
                visitationHelper = CreateInstance();
        }
        #endregion

        #region IEqualityComparer<object> Members
        bool IEqualityComparer<object>.Equals(object x, object y)
        {
            return ReferenceEquals(x, y);
        }

        int IEqualityComparer<object>.GetHashCode(object obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
        #endregion

        #region Public Methods
        public ObjectVisitationHelper Clone()
        {
            return CreateInstance(this);
        }

        public bool TryVisit(object obj)
        {
            if (IsVisited(obj))
                return false;

            _objectSet.Add(obj, obj);

            return true;
        }

        public bool IsVisited(object obj)
        {
            if (obj == null)
                throw new NullReferenceException("obj");

            return _objectSet.ContainsKey(obj);
        }

        public object FindVisited(object obj)
        {
            if (obj == null)
                throw new NullReferenceException("obj");

            object result;

            if (_objectSet.TryGetValue(obj, out result))
                return result;

            return null;
        }

        public void Reset()
        {
            _objectSet.Clear();
        }
        #endregion
    }
}

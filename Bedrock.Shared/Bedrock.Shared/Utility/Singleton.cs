using System;

namespace Bedrock.Shared.Utility
{
    public abstract class Singleton<T>
        where T : class, new()
    {
        #region Member Fields
        private static readonly Lazy<T> _current;
        #endregion

        #region Constructors
        static Singleton()
        {
            var instance = New<T>.Instance();
            _current = new Lazy<T>(() => instance);
        }
        #endregion

        #region Properties
        public static T Current { get { return _current.Value; } }
        #endregion
    }
}

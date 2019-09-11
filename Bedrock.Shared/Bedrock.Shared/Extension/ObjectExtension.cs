using System;

namespace Bedrock.Shared.Extension
{
    public static class ObjectExtension
    {
        #region Public Methods
        public static T GetPropertyValue<T>(this object instance, string propertyName)
        {
            return (T)instance
                        .GetType()
                        .GetProperty(propertyName)
                        .GetValue(instance);
        }

        public static bool As<T>(this object obj, Action<T> action) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var target = obj as T;
            if (target == null)
                return false;

            action(target);
            return true;
        }
        #endregion
    }
}

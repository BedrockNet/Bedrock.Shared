using System;

namespace Bedrock.Shared.Utility
{
    public static class TypeSwitch
    {
        #region Public Methods
        public static void On<T>(params CaseInfo[] cases)
        {
            var type = typeof(T);
            On(type, cases);
        }

        public static void On(Type t, params CaseInfo[] cases)
        {
            foreach (var entry in cases)
            {
                if (entry.IsDefault || entry.Target.IsAssignableFrom(t))
                {
                    entry.Action();
                    break;
                }
            }
        }

        public static CaseInfo Case<T>(Action action)
        {
            return new CaseInfo()
            {
                Action = action,
                Target = typeof(T)
            };
        }

        public static CaseInfo Default(Action action)
        {
            return new CaseInfo()
            {
                Action = action,
                IsDefault = true
            };
        }
        #endregion

        #region Classes
        public class CaseInfo
        {
            public Type Target { get; set; }

            public Action Action { get; set; }

            public bool IsDefault { get; set; }
        }
        #endregion
    }
}

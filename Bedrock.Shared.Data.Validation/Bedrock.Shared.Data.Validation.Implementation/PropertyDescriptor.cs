using System;
using System.Reflection;

using Bedrock.Shared.Data.Validation.Interface;

namespace Bedrock.Shared.Data.Validation.Implementation
{
    [Serializable]
    public class PropertyDescriptor : IPropertyDescriptor
    {
        #region Fields
        private PropertyInfo _info;
        #endregion

        #region Constructors
        public PropertyDescriptor(PropertyInfo info)
        {
            Name = info.Name;
            _info = info;
        }

        public PropertyDescriptor(string name)
        {
            Name = name;
        }
        #endregion

        #region Properties
        public string Name { get; private set; }
        #endregion

        #region Methods
        public object GetValue(object target)
        {
            return _info.GetValue(target, null);
        }
        #endregion
    }
}

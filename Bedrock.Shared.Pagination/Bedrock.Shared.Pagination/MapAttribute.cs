using System;

namespace Bedrock.Shared.Pagination
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class MapAttribute : Attribute
    {
        #region Fields
        private string _name;
        #endregion

        #region Constructors
        public MapAttribute() { }

        public MapAttribute(string name)
        {
            _name = name;
        }
        #endregion

        #region Properties
        public string Name
        {
            get { return _name; }
        }

        public bool HasName
        {
            get { return !string.IsNullOrEmpty(_name); }
        }
        #endregion
    }
}

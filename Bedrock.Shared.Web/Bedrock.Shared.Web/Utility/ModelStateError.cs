using System.Collections.Generic;
using Bedrock.Shared.Extension;

namespace Bedrock.Shared.Web.Utility
{
    public class ModelStateError
    {
        #region Public Properties
        public string Key { get; set; }

        public string Value { get; set; }
        #endregion

        #region Public Methods
        public static ModelStateError CreateInstance(string key, string value)
        {
            return new ModelStateError
            {
                Key = key,
                Value = value
            };
        }

        public static List<ModelStateError> CreateListFromStringValues(params string[] values)
        {
            var returnValue = new List<ModelStateError>();
            values.Each(v => returnValue.Add(new ModelStateError { Value = v }));
            return returnValue;
        }
        #endregion
    }
}

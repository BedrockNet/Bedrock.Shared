using System.Collections.Generic;
using System.Reflection;

namespace Bedrock.Shared.Utility
{
    public class PropertyValue
    {
        #region Public Methods
        public static bool GetValue(object currentObject, string propName, out object value)
        {
            return GetValue(currentObject, propName, out value, new HashSet<object>());
        }

        public static bool GetValueViaPath(object currentObject, string pathName, out object fieldValue)
        {
            var searchedObject = currentObject;
            var fieldNames = pathName.Split('.');

            foreach (string fieldName in fieldNames)
            {
                var curentRecordType = searchedObject.GetType();
                var property = curentRecordType.GetTypeInfo().GetProperty(fieldName);

                if (property != null)
                    searchedObject = property.GetValue(searchedObject, null).ToString();
                else
                {
                    fieldValue = null;
                    return false;
                }
            }

            fieldValue = searchedObject;

            return true;
        }
        #endregion

        #region Private Methods
        private static bool GetValue(object currentObject, string propName, out object value, HashSet<object> searchedObjects)
        {
            var propertyInfo = currentObject.GetType().GetTypeInfo().GetProperty(propName);

            if (propertyInfo != null)
            {
                value = propertyInfo.GetValue(currentObject, null);
                return true;
            }

            foreach (var property in currentObject.GetType().GetTypeInfo().GetProperties())
            {
                if (property.GetIndexParameters().Length == 0)
                {
                    object newObject = property.GetValue(currentObject, null);

                    if (newObject != null && searchedObjects.Add(newObject) && GetValue(newObject, propName, out value, searchedObjects))
                        return true;
                }
            }

            value = null;

            return false;
        }
        #endregion
    }
}

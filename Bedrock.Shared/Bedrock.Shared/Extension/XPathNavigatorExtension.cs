using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;

namespace Bedrock.Shared.Extension
{
    public static class XPathNavigatorExtension
    {
        #region Public Methods
        public static string GetString(this XPathNavigator nav, string key, object template = null, string defaultValue = null)
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<string>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (value.HasValue())
                returnValue = value;

            return returnValue;
        }

        public static bool GetBool(this XPathNavigator nav, string key, object template = null, bool defaultValue = false)
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<bool>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (value.HasValue())
                returnValue = bool.Parse(value);

            return returnValue;
        }

        public static bool? GetBoolNullable(this XPathNavigator nav, string key, object template = null, bool? defaultValue = null)
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<bool?>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (value.HasValue())
                returnValue = bool.Parse(value);

            return returnValue;
        }

        public static int GetInt(this XPathNavigator nav, string key, object template = null, int defaultValue = 0)
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<int>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (value.HasValue())
                returnValue = int.Parse(value);

            return returnValue;
        }

        public static int? GetIntNullable(this XPathNavigator nav, string key, object template = null, int? defaultValue = null)
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<int?>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (value.HasValue())
                returnValue = int.Parse(value);

            return returnValue;
        }

        public static T GetEnum<T>(this XPathNavigator nav, string key, object template = null, T defaultValue = default(T)) where T : struct
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<T>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (value.HasValue())
                returnValue = (T)Enum.Parse(typeof(T), value, true);

            return returnValue;
        }

        public static T? GetEnumNullable<T>(this XPathNavigator nav, string key, object template = null, T? defaultValue = null) where T : struct
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<T?>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (value.HasValue())
                returnValue = (T)Enum.Parse(typeof(T), value, true);

            return returnValue;
        }

        public static T GetFlags<T>(this XPathNavigator nav, string key, object template = null, T defaultValue = default(T)) where T : struct
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<T>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (value.HasValue())
            {
                var flags = default(int);
                var flagStrings = value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                flagStrings.Each(f => flags = (int)(object)flags | (int)(object)(T)Enum.Parse(typeof(T), f, true));
                returnValue = !flags.Equals(default(int)) ? (T)(object)flags : default(T);
            }

            return returnValue;
        }

        public static T? GetFlagsNullable<T>(this XPathNavigator nav, string key, object template = null, T? defaultValue = null) where T : struct
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<T?>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (value.HasValue())
            {
                var flags = default(int);
                var flagStrings = value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                flagStrings.Each(f => flags = (int)(object)flags | (int)(object)(T)Enum.Parse(typeof(T), f, true));
                returnValue = !flags.Equals(default(int)) ? (T)(object)flags : new T?();
            }

            return returnValue;
        }

        public static TimeSpan GetTimeSpan(this XPathNavigator nav, string key, object template = null, TimeSpan defaultValue = default(TimeSpan))
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<TimeSpan>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (value.HasValue())
                returnValue = TimeSpan.Parse(value);

            return returnValue;
        }

        public static TimeSpan? GetTimeSpanNullable(this XPathNavigator nav, string key, object template = null, TimeSpan? defaultValue = null)
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<TimeSpan?>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (value.HasValue())
                returnValue = TimeSpan.Parse(value);

            return returnValue;
        }

        public static List<string> GetListString(this XPathNavigator nav, string key, object template = null, List<string> defaultValue = null)
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<List<string>>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (value.HasValue())
                returnValue = new List<string>(value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            return returnValue;
        }

        public static List<T> GetListEnum<T>(this XPathNavigator nav, string key, object template = null, List<T> defaultValue = null) where T : struct
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<List<T>>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (value.HasValue())
            {
                var enumStrings = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                returnValue = enumStrings.Select(es => (T)Enum.Parse(typeof(T), es, true)).ToList();
            }

            return returnValue;
        }

        public static HashSet<string> GetHashSet(this XPathNavigator nav, string key, object template = null, bool isDefaultEmpty = false, HashSet<string> defaultValue = null)
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<HashSet<string>>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (isDefaultEmpty)
                returnValue = new HashSet<string>();

            if (value.HasValue())
                returnValue = new HashSet<string>(value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            return returnValue;
        }

        public static DateTimeOffset GetDateTimeOffset(this XPathNavigator nav, string key, object template = null, DateTimeOffset defaultValue = default(DateTimeOffset))
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<DateTimeOffset>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (value.HasValue())
                returnValue = DateTimeOffset.Parse(value);

            return returnValue;
        }

        public static DateTimeOffset? GetDateTimeOffsetNullable(this XPathNavigator nav, string key, object template = null, DateTimeOffset? defaultValue = null)
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<DateTimeOffset?>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (value.HasValue())
                returnValue = DateTimeOffset.Parse(value);

            return returnValue;
        }

        public static Version GetVersion(this XPathNavigator nav, string key, object template = null, Version defaultValue = null)
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<Version>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (value.HasValue())
                returnValue = new Version(value);

            return returnValue;
        }

        public static Uri GetUri(this XPathNavigator nav, string key, object template = null, Uri defaultValue = null)
        {
            var returnValue = defaultValue;

            if (template != null)
                returnValue = template.GetPropertyValue<Uri>(key);

            var value = nav.GetAttribute(key.ToLowerFirstLetter(), string.Empty);

            if (value.HasValue())
                returnValue = new Uri(value);

            return returnValue;
        }
        #endregion
    }
}

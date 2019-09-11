using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

using Bedrock.Shared.Entity.Implementation;
using Bedrock.Shared.Extension;

using Bedrock.Shared.Pagination;
using Bedrock.Shared.Pagination.Enumeration;

using Bedrock.Shared.Utility;

namespace Bedrock.Shared.Data.Repository.EntityFramework.Helper
{
    public static class DynamicQueryableHelper
    {
        #region Fields
        public static string[] DefaultSortFields = new[] { "Id" };
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns QueryString and Parmeter List suitable for use with the System.Linq.Dynamic.DynamicQueryable library.
        /// Generic collections (i.e., List&lt;string&gt;) will be ignored and must be handled seperately.
        /// Nested classes are skipped
        /// </summary>
        /// <typeparam name="T">Type of object to be searched</typeparam>
        /// <param name="searchObject">Object to be searched</param>
        /// <param name="queryString">Generated querystring</param>
        /// <param name="parameters">Generated parameters</param>
        /// <returns>True if usable parameters were found.</returns>
        public static bool ParseSearchVars<T>(T searchObject, ref string queryString, ref object[] parameters)
        {
            return ParseSearchVars(searchObject, null, ref queryString, ref parameters);
        }

        /// <summary>
        /// Returns QueryString and Parmeter List suitable for use with the System.Linq.Dynamic.DynamicQueryable library.
        /// Generic collections (i.e., List&lt;string&gt;) will be ignored and must be handled seperately.
        /// Nested classes are skipped
        /// </summary>
        /// <typeparam name="T">Type of object to be searched</typeparam>
        /// <param name="searchObject">Object to be searched</param>
        /// <param name="prefix">Prefix to affic to property name; can also use MapAttribute for deeper nesting</param>
        /// <param name="queryString">Generated querystring</param>
        /// <param name="parameters">Generated parameters</param>
        /// <returns>True if usable parameters were found.</returns>
        public static bool ParseSearchVars<T>(T searchObject, string prefix, ref string queryString, ref object[] parameters)
        {
            var parameterlist = new List<object>();
            var sourceType = searchObject.GetType();
            var columnPrefix = string.IsNullOrWhiteSpace(prefix) ? string.Empty : prefix + ".";

            queryString = string.Empty;

            if (sourceType.GetTypeInfo().IsGenericType)
                throw new ArgumentException($"ParseSearchVars() does not handle generics yet. Please pass each {sourceType.ToString()} as a seperate item to be parsed.");

            var position = 0;
            var properties = default(PropertyInfo[]);
            var flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;

            if (IsEntityType(sourceType))
                properties = ApplicationContext.DomainGraphCache.GetProperties(sourceType, flags).Values.ToArray();
            else
                properties = ApplicationContext.DomainGraphCache.GetProperties(sourceType).Values.ToArray();

            foreach (var property in properties)
            {
                var value = default(object);
                var equalPreProp = string.Empty;
                var equalPosProp = string.Empty;
                var propName = property.Name;
                var exactMatchRequired = false;
                var compareOperator = default(CompareOperator?);
                var isDateTime = false;

                PropertyValue.GetValue(searchObject, propName, out value);

                var stringValue = value as string;
                var isValidStringValue = stringValue != null && !string.IsNullOrEmpty(stringValue);

                if (value != null && (stringValue == null || (stringValue != null && isValidStringValue)))
                {
                    var valueType = value.GetType();

                    if (valueType.IsNested || property.IsDefined(typeof(IgnoreSearchFilterAttribute)))
                        continue;

                    exactMatchRequired = property.IsDefined(typeof(ExactMatchSearchFilterAttribute));

                    if (valueType.GetTypeInfo().IsGenericType && !(value is CriteriaField))
                        continue;

                    if (value is CriteriaField || valueType.FullName.Contains("CriteriaField"))
                    {
                        var propCvalue = ApplicationContext.DomainGraphCache.GetProperty(valueType, "Cvalue");
                        var propCriteria = ApplicationContext.DomainGraphCache.GetProperty(valueType, "Criteria");

                        if (propCvalue == null || propCriteria == null)
                            throw new FieldAccessException("CriteriaField is missing expected properties.");

                        var criteriaOperator = propCriteria.GetValue(value, null);
                        var criteriaValue = propCvalue.GetValue(value, null);

                        //  Reset for future check
                        value = criteriaValue;
                        compareOperator = criteriaOperator as Nullable<CompareOperator>;

                        if (value == null)
                            continue; //catch NULL values that were passed with generic CriteriaFields
                    }

                    if (property.IsDefined(typeof(MapAttribute)))
                    {
                        var attribute = property.GetCustomAttribute(typeof(MapAttribute)) as MapAttribute;
                        propName = string.IsNullOrEmpty(attribute.Name) ? propName : attribute.Name;
                    }

                    #region special handling of variables that get set to invalid values by default <Nullable>'s etc...
                    if (value is DateTime)
                    {
                        if (DateTime.MinValue.Equals(value) || DateTime.MaxValue.Equals(value))
                            continue;

                        var dateTimeValue = (DateTime)value;
                        value = dateTimeValue.Date;

                        isDateTime = true;
                    }

                    if (value is Nullable<DateTime>)
                    {
                        var dateTime = (Nullable<DateTime>)value;

                        if (!dateTime.HasValue || DateTime.MinValue.Equals(dateTime) || DateTime.MaxValue.Equals(dateTime))
                            continue;

                        value = dateTime.Value;
                        isDateTime = true;
                    }

                    if (value is Guid && Guid.Empty.Equals(value))
                        continue;

                    if (value is byte && ((byte)value == 0))
                        continue;

                    if (value is Int16 && ((Int16)value == 0))
                        continue;

                    if (value is Int32 && ((Int32)value == 0))
                        continue;

                    if (value is Int64 && ((Int64)value == 0))
                        continue;

                    if (value is decimal && ((decimal)value == 0))
                        continue;

                    if (value is byte?)
                    {
                        var tempValue = (byte?)value;

                        if (!tempValue.HasValue || tempValue == 0)
                            continue;

                        value = tempValue.Value;
                    }

                    if (value is Int16?)
                    {
                        var tempValue = (Int16?)value;

                        if (!tempValue.HasValue || tempValue == 0)
                            continue;

                        value = tempValue.Value;
                    }

                    if (value is Int32?)
                    {
                        var tempValue = (Int32?)value;

                        if (!tempValue.HasValue || tempValue == 0)
                            continue;

                        value = tempValue.Value;
                    }

                    if (value is Int64?)
                    {
                        var tempValue = (Int64?)value;

                        if (!tempValue.HasValue || tempValue == 0)
                            continue;

                        value = tempValue.Value;
                    }

                    if (value is decimal?)
                    {
                        var tempValue = (decimal?)value;

                        if (!tempValue.HasValue || tempValue == 0)
                            continue;

                        value = tempValue.Value;
                    }
                    #endregion

                    if (queryString.Length > 0)
                        queryString += " and ";

                    propName = isDateTime ? $"{propName}.Date" : propName;

                    switch (compareOperator)
                    {
                        case CompareOperator.Equal:
                            queryString += string.Format("{4}{3}{0}{5} {1} @{2}", propName, "=", position, columnPrefix, equalPreProp, equalPosProp);
                            break;
                        case CompareOperator.GreaterThan:
                            queryString += string.Format("{3}{0} {1} @{2}", propName, ">", position, columnPrefix);
                            break;
                        case CompareOperator.GreaterThanOrEqual:
                            queryString += string.Format("{3}{0} {1} @{2}", propName, ">=", position, columnPrefix);
                            break;
                        case CompareOperator.LessThanOrEqual:
                            queryString += string.Format("{3}{0} {1} @{2}", propName, "<=", position, columnPrefix);
                            break;
                        case CompareOperator.LessThan:
                            queryString += string.Format("{3}{0} {1} @{2}", propName, "<", position, columnPrefix);
                            break;
                        default:
                            if (exactMatchRequired)
                                queryString += string.Format("{2}{0}=@{1}", propName, position, columnPrefix);
                            else
                                queryString += string.Format("{2}{0}.Contains(@{1})", propName, position, columnPrefix);
                            break;
                    }

                    parameterlist.Add(value);
                    position++;
                }
            }

            parameters = parameterlist.ToArray();

            return parameterlist.Count > 0;
        }

        public static IQueryable<T> ProcessSearchSettings<T, S>(PagingInstruction pager, IQueryable<T> query)
        {
            if (pager == null)
                return query;

            query = ProcessSearchSorting<T, S>(pager, query);
            query = ProcessSearchPaging<T>(pager, query);

            return query;
        }

        public static IQueryable<TDatasource> ValidatePaging<TDatasource>(PagingInstruction pager, IQueryable<TDatasource> queryable) where TDatasource : class
        {
            var skip = 0;
            var take = 0;

            if (pager == null)
                return queryable;

            pager.TotalRowCount = queryable.Count();

            if (pager.TotalRowCount == 0)
                return queryable;

            if (pager.PageSize > 0)
            {
                if (pager.PageIndex > Math.Round(pager.TotalRowCount / (decimal)pager.PageSize, MidpointRounding.AwayFromZero))
                {
                    pager.PageIndex = Convert.ToInt32(Math.Round(pager.TotalRowCount / (decimal)pager.PageSize, MidpointRounding.AwayFromZero));
                }

                skip = pager.PageIndex * pager.PageSize;
                take = pager.PageSize;
            }

            var t = typeof(TDatasource);

            if (pager.SortOptions == null || pager.SortOptions.Count == 0)
            {
                pager.SortOptions = new List<SortingInstruction>();
                var columnName = default(string);

                // Attempt to assign a default sort because the client doesn't seem to care.
                if (t.GetProperties().Where(pname => DefaultSortFields.Contains(pname.Name)).Select(x => x.Name).Count() > 0)
                {
                    columnName = t.GetProperties().Where(pname => DefaultSortFields.Contains(pname.Name)).Select(x => x.Name).First();
                }
                else
                {
                    var destElement = Activator.CreateInstance<TDatasource>();
                    var list = t.GetProperties(BindingFlags.Public | BindingFlags.Instance).AsQueryable();

                    if (list.Count() > 0)
                        columnName = list.First().Name;
                }

                if (string.IsNullOrWhiteSpace(columnName))
                    throw new Exception("The Object does not seem to have any sort fields. This is required in order to handle paging correctly.");

                pager.SortOptions.Add(new SortingInstruction() { Column = columnName });
            }

            var sortByString = new StringBuilder();

            foreach (var sortby in pager.SortOptions)
            {
                if (sortByString.Length > 0)
                    sortByString.Append(",");

                var columnName = sortby.Column;
                sortByString.AppendFormat("{0} {1}", columnName, sortby.Direction);
            }

            queryable = queryable.OrderBy(sortByString.ToString());

            if (pager.PageIndex < 0)
                pager.PageIndex = 0;

            if (pager.PageSize <= 0)
                pager.PageSize = pager.TotalRowCount;

            if (skip > 0)
                queryable = queryable.Skip(skip);

            if (take > 0)
                queryable = queryable.Take(take);

            return queryable;
        }
        #endregion

        #region Private Methods
        private static IQueryable<T> ProcessSearchSorting<T, S>(PagingInstruction pager, IQueryable<T> query)
        {
            var type = typeof(T);

            if (pager.SortOptions == null || pager.SortOptions.Count == 0)
            {
                pager.SortOptions = new List<SortingInstruction>();
                var columnName = default(string);

                if (type.GetProperties().Where(pname => DefaultSortFields.Contains(pname.Name)).Select(x => x.Name).Count() > 0)
                {
                    columnName = type.GetProperties().Where(pname => DefaultSortFields.Contains(pname.Name)).Select(x => x.Name).First();
                }

                if (string.IsNullOrWhiteSpace(columnName))
                    throw new Exception("The Object does not seem to have any fields to use for sorting. This is required in order to handle paging correctly.");

                pager.SortOptions.Add(new SortingInstruction() { Column = columnName, Direction = SortOrder.Ascending });
            }

            var sortString = new StringBuilder();

            foreach (var sortBy in pager.SortOptions)
            {
                if (sortString.Length > 0)
                    sortString.Append(",");

                type = typeof(S);

                var columnName = sortBy.Column;
                var property = type.GetProperty(columnName);

                if(property == null)
                {
                    property = type.GetProperty(columnName.ToUpperFirstLetter());
                }

                if (property == null)
                {
                    type = typeof(T);
                    property = type.GetProperty(columnName);
                }

                if (property == null)
                {
                    property = type.GetProperty(columnName.ToUpperFirstLetter());
                }

                if (property == null)
                    continue;

                var attribute = property.GetCustomAttribute<MapAttribute>(true);

                if (attribute != null)
                    columnName = attribute.Name;

                sortString.AppendFormat("{0} {1}", columnName, sortBy.Direction);
            }

            return query.OrderBy(sortString.ToString());
        }

        private static IQueryable<T> ProcessSearchPaging<T>(PagingInstruction pager, IQueryable<T> query)
        {
            var skip = 0;
            var take = 0;

            pager.TotalRowCount = query.Count();

            if (pager.TotalRowCount == 0)
                return query;

            if (pager.PageSize > 0)
            {
                if (pager.PageIndex > Math.Round(pager.TotalRowCount / (decimal)pager.PageSize, MidpointRounding.AwayFromZero))
                {
                    pager.PageIndex = Convert.ToInt32(Math.Round(pager.TotalRowCount / (decimal)pager.PageSize, MidpointRounding.AwayFromZero));
                }

                skip = pager.PageIndex * pager.PageSize;
                take = pager.PageSize;
            }

            if (pager.PageIndex < 0)
                pager.PageIndex = 0;

            if (pager.PageSize <= 0)
                pager.PageSize = pager.TotalRowCount;

            if (skip > 0)
                query = query.Skip<T>(skip);

            if (take > 0)
                query = query.Take<T>(take);

            return query;
        }

        private static bool IsEntityType(Type sourceType)
        {
            return
            (
                sourceType.GetTypeInfo().BaseType != null &&
                sourceType.GetTypeInfo().BaseType.GetTypeInfo().IsGenericType &&
                (sourceType.GetTypeInfo().BaseType.GetGenericTypeDefinition() == typeof(BedrockEntity<>) || sourceType.GetTypeInfo().BaseType.GetGenericTypeDefinition() == typeof(BedrockIdEntity<,>))
            );
        }
        #endregion
    }
}

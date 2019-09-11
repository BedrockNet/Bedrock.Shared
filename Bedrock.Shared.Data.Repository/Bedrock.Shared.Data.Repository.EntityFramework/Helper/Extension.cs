using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;

using Microsoft.Extensions.DependencyInjection;

namespace Bedrock.Shared.Data.Repository.EntityFramework.Helper
{
    public static class Extension
    {
        #region Public Methods
        public static IEnumerable<T> GetModelFromQuery<T>(this DatabaseFacade databaseFacade, string sql, params object[] parameters)
            where T : new()
        {
            using (var dataReader = databaseFacade.ExecuteSqlQuery(sql, parameters))
                return Hydrate<T>(dataReader.DbDataReader);
        }

        public static async Task<IEnumerable<T>> GetModelFromQueryAsync<T>(this DatabaseFacade databaseFacade, string sql, CancellationToken cancellationToken = default(CancellationToken), params object[] parameters)
            where T : new()
        {
            using (var dataReader = await databaseFacade.ExecuteSqlCommandAsync(sql, cancellationToken, parameters))
                return Hydrate<T>(dataReader.DbDataReader);
        }
        #endregion

        #region Private Methods
        private static IEnumerable<T> Hydrate<T>(DbDataReader dataReader) where T : new()
        {
            var returnValue = new List<T>();
            var properties = typeof(T).GetTypeInfo().GetProperties();

            while (dataReader.Read())
            {
                var t = new T();
                var actualNames = dataReader
                                    .GetColumnSchema()
                                    .Select(o => o.ColumnName);

                for (int i = 0; i < properties.Length; ++i)
                {
                    var propertyInfo = properties[i];

                    if (!propertyInfo.CanWrite)
                        continue;

                    var customAttribute = propertyInfo.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;
                    var name = customAttribute?.Name ?? propertyInfo.Name;

                    if (propertyInfo == null)
                        continue;

                    if (!actualNames.Contains(name))
                        continue;

                    var value = dataReader[name];
                    var propertyInfoType = propertyInfo.DeclaringType;
                    var nullable = propertyInfoType.GetTypeInfo().IsGenericType && propertyInfoType.GetGenericTypeDefinition() == typeof(Nullable<>);

                    if (value == DBNull.Value)
                        value = null;

                    if (value == null && propertyInfoType.GetTypeInfo().IsValueType && !nullable)
                        value = Activator.CreateInstance(propertyInfoType);

                    propertyInfo.SetValue(t, value);
                }

                returnValue.Add(t);
            }

            return returnValue;
        }

        private static RelationalDataReader ExecuteSqlQuery(this DatabaseFacade databaseFacade, string sql, params object[] parameters)
        {
            var concurrencyDetector = databaseFacade.GetService<IConcurrencyDetector>();

            using (concurrencyDetector.EnterCriticalSection())
            {
                var rawSqlCommand = databaseFacade
                                    .GetService<IRawSqlCommandBuilder>()
                                    .Build(sql, parameters);

                return rawSqlCommand
                        .RelationalCommand
                        .ExecuteReader(
                            databaseFacade.GetService<IRelationalConnection>(),
                            parameterValues: rawSqlCommand.ParameterValues);
            }
        }

        private static async Task<RelationalDataReader> ExecuteSqlCommandAsync(this DatabaseFacade databaseFacade, string sql, CancellationToken cancellationToken = default(CancellationToken), params object[] parameters)
        {
            var concurrencyDetector = databaseFacade.GetService<IConcurrencyDetector>();

            using (concurrencyDetector.EnterCriticalSection())
            {
                var rawSqlCommand = databaseFacade
                                    .GetService<IRawSqlCommandBuilder>()
                                    .Build(sql, parameters);

                return await rawSqlCommand
                                .RelationalCommand
                                .ExecuteReaderAsync(
                                    databaseFacade.GetService<IRelationalConnection>(),
                                    parameterValues: rawSqlCommand.ParameterValues,
                                    cancellationToken: cancellationToken);
            }
        }
        #endregion
    }
}

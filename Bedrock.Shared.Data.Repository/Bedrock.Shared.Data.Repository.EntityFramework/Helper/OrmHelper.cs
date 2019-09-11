using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Data.Repository.Interface;
using Bedrock.Shared.Entity.Interface;
using Bedrock.Shared.Mapper.Extension;
using Bedrock.Shared.Pagination;
using Bedrock.Shared.Pagination.Enumeration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

using Extensions = Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;

namespace Bedrock.Shared.Data.Repository.EntityFramework.Helper
{
    public class OrmHelper : IOrmHelper
    {
        #region IOrmHelper Methods
        public IIncludeQueryable<TEntity, TProperty> Include<TEntity, TProperty>(IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> path) where TEntity : class where TProperty : class
        {
            query = query.Include(path);
            return new IncludeQueryable<TEntity, TProperty>(query);
        }

        public IIncludeQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(IIncludeQueryable<TEntity, IEnumerable<TPreviousProperty>> query, Expression<Func<TPreviousProperty, TProperty>> path) where TEntity : class
        {
            var propertyName = ((MemberExpression)path.Body).Member.Name;
            var previousPropertyExpression = Expression.Parameter(typeof(TPreviousProperty));
            var includingPropertyExpression = Expression.Convert(Expression.Property(previousPropertyExpression, propertyName), typeof(TProperty));
            var includeQuery = query.Queryable as IIncludableQueryable<TEntity, ICollection<TPreviousProperty>>;
            var queryable = includeQuery.ThenInclude(Expression.Lambda<Func<TPreviousProperty, TProperty>>(includingPropertyExpression, previousPropertyExpression));

            return new IncludeQueryable<TEntity, TProperty>(queryable);
        }

        public IIncludeQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(IIncludeQueryable<TEntity, TPreviousProperty> query, Expression<Func<TPreviousProperty, TProperty>> path) where TEntity : class
        {
            var propertyName = ((MemberExpression)path.Body).Member.Name;
            var previousPropertyExpression = Expression.Parameter(typeof(TPreviousProperty));
            var includingPropertyExpression = Expression.Convert(Expression.Property(previousPropertyExpression, propertyName), typeof(TProperty));
            var includeQuery = query.Queryable as IIncludableQueryable<TEntity, TPreviousProperty>;
            var queryable = includeQuery.ThenInclude(Expression.Lambda<Func<TPreviousProperty, TProperty>>(includingPropertyExpression, previousPropertyExpression));

            return new IncludeQueryable<TEntity, TProperty>(queryable);
        }

        public IQueryable<TEntity> FindBy<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, bool>> predicate)
        {
            return query.Where(predicate);
        }

        public TEntity GetSingle<TEntity, TKey>(IQueryable<TEntity> query, TKey id) where TEntity : class, IBedrockEntity, IBedrockIdEntity<TKey> where TKey : IComparable
        {
            return EntityHelper.Filter(query, x => x.Id, id).FirstOrDefault();
        }
        #endregion

        #region IOrmHelper Methods (Pagination)

        public PaginationResult<TEntity> Paginate<TEntity>(IQueryable<TEntity> query,
                                                            int pageIndex,
                                                            int pageSize,
                                                            Expression<Func<TEntity, object>> sortField,
                                                            SortOrder sortOrder,
                                                            Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            var pagination = PaginateInternal(query, pageIndex, pageSize, sortField, sortOrder, predicate);
            var count = pagination.Item1.Count();
            var data = pagination.Item2.ToList();

            return new PaginationResult<TEntity>(data, pageIndex, pageSize, count);
        }

        public async Task<PaginationResult<TEntity>> PaginateAsync<TEntity>(IQueryable<TEntity> query,
                                                                                int pageIndex,
                                                                                int pageSize,
                                                                                Expression<Func<TEntity, object>> sortField,
                                                                                SortOrder sortOrder,
                                                                                Expression<Func<TEntity, bool>> predicate,
                                                                                CancellationToken cancellationToken = default(CancellationToken)) where TEntity : class
        {
            var pagination = PaginateInternal(query, pageIndex, pageSize, sortField, sortOrder, predicate);
            var count = await CountAsync(pagination.Item1);
            var data = await ToListAsync(pagination.Item2);

            return new PaginationResult<TEntity>(data, pageIndex, pageSize, count);
        }

        public PaginationResult<TEntity> Paginate<TEntity, TSearch>(IQueryable<TEntity> query, TSearch searchObject, PagingInstruction pagingInstruction, Expression<Func<TEntity, bool>> predicate = null, string prefix = null)
        {
            var results = PaginateInternal(query, searchObject, pagingInstruction, predicate, prefix);
            var totalCount = pagingInstruction.TotalRowCount;
            var data = results.Item2.ToList();

            return new PaginationResult<TEntity>
            (
                data,
                pagingInstruction.PageIndex,
                pagingInstruction.PageSize,
                totalCount
            );
        }

        public async Task<PaginationResult<TEntity>> PaginateAsync<TEntity, TSearch>(IQueryable<TEntity> query, TSearch searchObject, PagingInstruction pagingInstruction, Expression<Func<TEntity, bool>> predicate = null, string prefix = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var results = PaginateInternal(query, searchObject, pagingInstruction, predicate, prefix);
            var totalCount = pagingInstruction.TotalRowCount;
            var data = await ToListAsync(results.Item2, cancellationToken);

            return new PaginationResult<TEntity>
            (
                data,
                pagingInstruction.PageIndex,
                pagingInstruction.PageSize,
                totalCount
            );
        }

        public PaginationResult<TContract> Paginate<TEntity, TContract, TSearch>(IQueryable<TEntity> query, TSearch searchObject, PagingInstruction pagingInstruction, Expression<Func<TEntity, bool>> predicate = null, string prefix = null)
        {
            var results = PaginateInternal(query, searchObject, pagingInstruction, predicate, prefix);
            var totalCount = pagingInstruction.TotalRowCount;

            var data = MapperExtension
                        .ProjectTo<TEntity, TContract>(results.Item2)
                        .ToList();

            return new PaginationResult<TContract>
            (
                data,
                pagingInstruction.PageIndex,
                pagingInstruction.PageSize,
                totalCount
            );
        }

        public async Task<PaginationResult<TContract>> PaginateAsync<TEntity, TContract, TSearch>(IQueryable<TEntity> query, TSearch searchObject, PagingInstruction pagingInstruction, Expression<Func<TEntity, bool>> predicate = null, string prefix = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var results = PaginateInternal(query, searchObject, pagingInstruction, predicate, prefix);
            var totalCount = pagingInstruction.TotalRowCount;
            var data = await ToListAsync(MapperExtension.ProjectTo<TEntity, TContract>(results.Item2), cancellationToken);

            return new PaginationResult<TContract>
            (
                data,
                pagingInstruction.PageIndex,
                pagingInstruction.PageSize,
                totalCount
            );
        }
		#endregion

		#region IOrmHelper (DynamicQueryable)
		public bool ParseSearchVars<T>(T searchObject, ref string queryString, ref object[] parameters)
		{
			return DynamicQueryableHelper.ParseSearchVars(searchObject, ref queryString, ref parameters);
		}

		public bool ParseSearchVars<T>(T searchObject, string prefix, ref string queryString, ref object[] parameters)
		{
			return DynamicQueryableHelper.ParseSearchVars(searchObject, prefix, ref queryString, ref parameters);
		}

		public IQueryable<T> ProcessSearchSettings<T, S>(PagingInstruction pager, IQueryable<T> query)
		{
			return DynamicQueryableHelper.ProcessSearchSettings<T, S>(pager, query);
		}

		public IQueryable<TDatasource> ValidatePaging<TDatasource>(PagingInstruction pager, IQueryable<TDatasource> queryable) where TDatasource : class
		{
			return DynamicQueryableHelper.ValidatePaging(pager, queryable);

		}

		public IQueryable<TEntity> Where<TEntity>(IQueryable<TEntity> query, string predicate, params object[] arguments)
		{
			return query.Where(predicate, arguments);
		}
		#endregion

		#region IOrmHelper (Async)
		public Task<bool> AllAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AllAsync<T>(source, predicate, cancellationToken);
        }

        public Task<bool> AnyAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AnyAsync<T>(source, cancellationToken);
        }

        public Task<bool> AnyAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AnyAsync<T>(source, predicate, cancellationToken);
        }

        public IQueryable<TEntity> AsNoTracking<TEntity>(IQueryable<TEntity> source) where TEntity : class
        {
            return Extensions.AsNoTracking(source);
        }
       
        public IQueryable<TEntity> AsTracking<TEntity>(IQueryable<TEntity> source) where TEntity : class
        {
            return Extensions.AsTracking(source);
        }

        public Task<double> AverageAsync(IQueryable<int> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, cancellationToken);
        }

        public Task<double?> AverageAsync(IQueryable<int?> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, cancellationToken);
        }

        public Task<double> AverageAsync(IQueryable<long> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, cancellationToken);
        }

        public Task<double?> AverageAsync(IQueryable<long?> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, cancellationToken);
        }

        public Task<double> AverageAsync(IQueryable<double> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, cancellationToken);
        }

        public Task<double?> AverageAsync(IQueryable<double?> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, cancellationToken);
        }

        public Task<decimal> AverageAsync(IQueryable<decimal> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, cancellationToken);
        }

        public Task<decimal?> AverageAsync(IQueryable<decimal?> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, cancellationToken);
        }

        public Task<float> AverageAsync(IQueryable<float> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, cancellationToken);
        }

        public Task<float?> AverageAsync(IQueryable<float?> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, cancellationToken);
        }

        public Task<double> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, int>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, predicate, cancellationToken);
        }

        public Task<double?> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, int?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, predicate, cancellationToken);
        }

        public Task<double> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, long>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, predicate, cancellationToken);
        }

        public Task<double?> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, long?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, predicate, cancellationToken);
        }

        public Task<double> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, double>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, predicate, cancellationToken);
        }

        public Task<double?> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, double?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, predicate, cancellationToken);
        }

        public Task<decimal> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, decimal>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, predicate, cancellationToken);
        }

        public Task<decimal?> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, decimal?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, predicate, cancellationToken);
        }

        public Task<float> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, float>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, predicate, cancellationToken);
        }

        public Task<float?> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, float?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.AverageAsync(source, predicate, cancellationToken);
        }

        public Task<bool> ContainsAsync<T>(IQueryable<T> source, T item, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.ContainsAsync(source, item, cancellationToken);
        }

        public Task<int> CountAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.CountAsync(source, cancellationToken);
        }

        public Task<int> CountAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.CountAsync(source, predicate, cancellationToken);
        }

        public Task<T> FirstAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.FirstAsync(source, cancellationToken);
        }

        public Task<T> FirstAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.FirstAsync(source, predicate, cancellationToken);
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.FirstOrDefaultAsync(source, cancellationToken);
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.FirstOrDefaultAsync(source, predicate, cancellationToken);
        }

        public Task ForEachAsync<TSource>(IQueryable<TSource> source, Action<TSource> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.ForEachAsync(source, action, cancellationToken);
        }

        public Task<TSource> LastAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.LastAsync(source, cancellationToken);
        }
        
        public Task<TSource> LastAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.LastAsync(source, predicate, cancellationToken);
        }
        
        public Task<TSource> LastOrDefaultAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.LastOrDefaultAsync(source, cancellationToken);
        }
        
        public Task<TSource> LastOrDefaultAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.LastOrDefaultAsync(source, predicate, cancellationToken);
        }

        public void Load<TSource>(IQueryable<TSource> source)
        {
            Extensions.Load(source);
        }

        public Task LoadAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.LoadAsync(source, cancellationToken);
        }

        public Task<long> LongCountAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.LongCountAsync(source, cancellationToken);
        }

        public Task<long> LongCountAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.LongCountAsync(source, predicate, cancellationToken);
        }

        public Task<T> MaxAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.MaxAsync(source, cancellationToken);
        }

        public Task<T> MaxAsync<T, R>(IQueryable<T> source, Expression<Func<T, R>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.MaxAsync(source, cancellationToken);
        }

        public Task<T> MinAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.MinAsync(source, cancellationToken);
        }

        public Task<T> MinAsync<T, R>(IQueryable<T> source, Expression<Func<T, R>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.MinAsync(source, cancellationToken);
        }

        public Task<T> SingleAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SingleAsync(source, cancellationToken);
        }

        public Task<T> SingleAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SingleAsync(source, predicate, cancellationToken);
        }

        public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SingleOrDefaultAsync(source, cancellationToken);
        }

        public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SingleOrDefaultAsync(source, predicate, cancellationToken);
        }

        public Task<int> SumAsync(IQueryable<int> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, cancellationToken);
        }

        public Task<int?> SumAsync(IQueryable<int?> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, cancellationToken);
        }

        public Task<long> SumAsync(IQueryable<long> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, cancellationToken);
        }

        public Task<long?> SumAsync(IQueryable<long?> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, cancellationToken);
        }

        public Task<double> SumAsync(IQueryable<double> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, cancellationToken);
        }

        public Task<double?> SumAsync(IQueryable<double?> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, cancellationToken);
        }

        public Task<decimal> SumAsync(IQueryable<decimal> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, cancellationToken);
        }

        public Task<decimal?> SumAsync(IQueryable<decimal?> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, cancellationToken);
        }

        public Task<float> SumAsync(IQueryable<float> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, cancellationToken);
        }

        public Task<float?> SumAsync(IQueryable<float?> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, cancellationToken);
        }

        public Task<int> SumAsync<T>(IQueryable<T> source, Expression<Func<T, int>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, predicate, cancellationToken);
        }

        public Task<int?> SumAsync<T>(IQueryable<T> source, Expression<Func<T, int?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, predicate, cancellationToken);
        }

        public Task<long> SumAsync<T>(IQueryable<T> source, Expression<Func<T, long>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, predicate, cancellationToken);
        }

        public Task<long?> SumAsync<T>(IQueryable<T> source, Expression<Func<T, long?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, predicate, cancellationToken);
        }

        public Task<double> SumAsync<T>(IQueryable<T> source, Expression<Func<T, double>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, predicate, cancellationToken);
        }

        public Task<double?> SumAsync<T>(IQueryable<T> source, Expression<Func<T, double?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, predicate, cancellationToken);
        }

        public Task<decimal> SumAsync<T>(IQueryable<T> source, Expression<Func<T, decimal>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, predicate, cancellationToken);
        }

        public Task<decimal?> SumAsync<T>(IQueryable<T> source, Expression<Func<T, decimal?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, predicate, cancellationToken);
        }

        public Task<float> SumAsync<T>(IQueryable<T> source, Expression<Func<T, float>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, predicate, cancellationToken);
        }

        public Task<float?> SumAsync<T>(IQueryable<T> source, Expression<Func<T, float?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.SumAsync(source, predicate, cancellationToken);
        }

        public Task<T[]> ToArrayAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.ToArrayAsync<T>(source, cancellationToken);
        }

        public Task<Dictionary<K, T>> ToDictionaryAsync<T, K>(IQueryable<T> source, Func<T, K> keySelector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.ToDictionaryAsync(source, keySelector, cancellationToken);
        } 

        public Task<Dictionary<K, T>> ToDictionaryAsync<T, K>(IQueryable<T> source, Func<T, K> keySelector, IEqualityComparer<K> comparer, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.ToDictionaryAsync(source, keySelector, comparer, cancellationToken);
        }

        public Task<Dictionary<K, E>> ToDictionaryAsync<T, K, E>(IQueryable<T> source, Func<T, K> keySelector, Func<T, E> elementSelector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.ToDictionaryAsync(source, keySelector, elementSelector, cancellationToken);
        }

        public Task<Dictionary<K, E>> ToDictionaryAsync<T, K, E>(IQueryable<T> source, Func<T, K> keySelector, Func<T, E> elementSelector, IEqualityComparer<K> comparer, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.ToDictionaryAsync(source, keySelector, elementSelector, comparer, cancellationToken);
        }

        public Task<List<TSource>> ToListAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Extensions.ToListAsync(source, cancellationToken);
        }
        #endregion

        #region Private Methods
        private Tuple<IQueryable<TEntity>, IQueryable<TEntity>> PaginateInternal<TEntity>(IQueryable<TEntity> query,
                                                int pageIndex,
                                                int pageSize,
                                                Expression<Func<TEntity, object>> sortField,
                                                SortOrder sortOrder,
                                                Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            query = (predicate != null) ? query.Where(predicate) : query;

            var collection = query.Skip(pageIndex * pageSize).Take(pageSize);

            if (sortField != null)
            {
                collection = (sortOrder == SortOrder.Ascending)
                            ? collection.OrderBy(sortField)
                            : collection.OrderByDescending(sortField);
            }

            return new Tuple<IQueryable<TEntity>, IQueryable<TEntity>>(query, collection);
        }

        private Tuple<IQueryable<TEntity>, IQueryable<TEntity>> PaginateInternal<TEntity, TSearch>(IQueryable<TEntity> query, TSearch searchObject, PagingInstruction pagingInstruction, Expression<Func<TEntity, bool>> predicate = null, string prefix = null)
        {
            var queryString = string.Empty;
            var parameters = new object[] { };

            if (DynamicQueryableHelper.ParseSearchVars(searchObject, ref queryString, ref parameters))
                query = query.Where(queryString, parameters);

            if (predicate != null)
                query = query.Where(predicate);

            var collection = DynamicQueryableHelper.ProcessSearchSettings<TEntity, TSearch>(pagingInstruction, query);

            return new Tuple<IQueryable<TEntity>, IQueryable<TEntity>>(query, collection);
        }
        #endregion
    }
}

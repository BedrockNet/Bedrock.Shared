using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Entity.Interface;

using Bedrock.Shared.Pagination;
using Bedrock.Shared.Pagination.Enumeration;

namespace Bedrock.Shared.Data.Repository.Interface
{
	public interface IOrmHelper
	{
		#region Methods
		IIncludeQueryable<TEntity, TProperty> Include<TEntity, TProperty>(IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> path) where TEntity : class where TProperty : class;

		IIncludeQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(IIncludeQueryable<TEntity, IEnumerable<TPreviousProperty>> query, Expression<Func<TPreviousProperty, TProperty>> path) where TEntity : class;

		IIncludeQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(IIncludeQueryable<TEntity, TPreviousProperty> query, Expression<Func<TPreviousProperty, TProperty>> path) where TEntity : class;

		IQueryable<TEntity> FindBy<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, bool>> predicate);

		TEntity GetSingle<TEntity, TKey>(IQueryable<TEntity> query, TKey id) where TEntity : class, IBedrockEntity, IBedrockIdEntity<TKey> where TKey : IComparable;
		#endregion

		#region Methods (Pagination)
		PaginationResult<TEntity> Paginate<TEntity>(IQueryable<TEntity> query, int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortField, SortOrder sortOrder, Expression<Func<TEntity, bool>> predicate) where TEntity : class;

		Task<PaginationResult<TEntity>> PaginateAsync<TEntity>(IQueryable<TEntity> query, int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortField, SortOrder sortOrder, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) where TEntity : class;

		PaginationResult<TEntity> Paginate<TEntity, TSearch>(IQueryable<TEntity> query, TSearch searchObject, PagingInstruction pagingInstruction, Expression<Func<TEntity, bool>> predicate = null, string prefix = null);

		Task<PaginationResult<TEntity>> PaginateAsync<TEntity, TSearch>(IQueryable<TEntity> query, TSearch searchObject, PagingInstruction pagingInstruction, Expression<Func<TEntity, bool>> predicate = null, string prefix = null, CancellationToken cancellationToken = default(CancellationToken));

		PaginationResult<TContract> Paginate<TEntity, TContract, TSearch>(IQueryable<TEntity> query, TSearch searchObject, PagingInstruction pagingInstruction, Expression<Func<TEntity, bool>> predicate = null, string prefix = null);

		Task<PaginationResult<TContract>> PaginateAsync<TEntity, TContract, TSearch>(IQueryable<TEntity> query, TSearch searchObject, PagingInstruction pagingInstruction, Expression<Func<TEntity, bool>> predicate = null, string prefix = null, CancellationToken cancellationToken = default(CancellationToken));
		#endregion

		#region Methods (DynamicQueryable)
		bool ParseSearchVars<T>(T searchObject, ref string queryString, ref object[] parameters);

		bool ParseSearchVars<T>(T searchObject, string prefix, ref string queryString, ref object[] parameters);

		IQueryable<T> ProcessSearchSettings<T, S>(PagingInstruction pager, IQueryable<T> query);

		IQueryable<TDatasource> ValidatePaging<TDatasource>(PagingInstruction pager, IQueryable<TDatasource> queryable) where TDatasource : class;

		IQueryable<TEntity> Where<TEntity>(IQueryable<TEntity> query, string predicate, params object[] arguments);
		#endregion

		#region Methods (Async)
		Task<bool> AllAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<bool> AnyAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<bool> AnyAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		IQueryable<TEntity> AsNoTracking<TEntity>(IQueryable<TEntity> source) where TEntity : class;

		IQueryable<TEntity> AsTracking<TEntity>(IQueryable<TEntity> source) where TEntity : class;

		Task<double> AverageAsync(IQueryable<int> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<double?> AverageAsync(IQueryable<int?> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<double> AverageAsync(IQueryable<long> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<double?> AverageAsync(IQueryable<long?> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<double> AverageAsync(IQueryable<double> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<double?> AverageAsync(IQueryable<double?> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<decimal> AverageAsync(IQueryable<decimal> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<decimal?> AverageAsync(IQueryable<decimal?> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<float> AverageAsync(IQueryable<float> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<float?> AverageAsync(IQueryable<float?> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<double> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, int>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<double?> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, int?>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<double> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, long>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<double?> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, long?>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<double> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, double>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<double?> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, double?>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<decimal> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, decimal>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<decimal?> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, decimal?>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<float> AverageAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, float>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<float?> AverageAsync<T>(IQueryable<T> source, Expression<Func<T, float?>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<bool> ContainsAsync<T>(IQueryable<T> source, T item, CancellationToken cancellationToken = default(CancellationToken));

		Task<int> CountAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<int> CountAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<T> FirstAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<T> FirstAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<T> FirstOrDefaultAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<T> FirstOrDefaultAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task ForEachAsync<TSource>(IQueryable<TSource> source, Action<TSource> action, CancellationToken cancellationToken = default(CancellationToken));

		Task<TSource> LastAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<TSource> LastAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<TSource> LastOrDefaultAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<TSource> LastOrDefaultAsync<TSource>(IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		void Load<TSource>(IQueryable<TSource> source);

		Task LoadAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<long> LongCountAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<long> LongCountAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<T> MaxAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<T> MaxAsync<T, R>(IQueryable<T> source, Expression<Func<T, R>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<T> MinAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<T> MinAsync<T, R>(IQueryable<T> source, Expression<Func<T, R>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<T> SingleAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<T> SingleAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<T> SingleOrDefaultAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<T> SingleOrDefaultAsync<T>(IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<int> SumAsync(IQueryable<int> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<int?> SumAsync(IQueryable<int?> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<long> SumAsync(IQueryable<long> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<long?> SumAsync(IQueryable<long?> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<double> SumAsync(IQueryable<double> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<double?> SumAsync(IQueryable<double?> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<decimal> SumAsync(IQueryable<decimal> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<decimal?> SumAsync(IQueryable<decimal?> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<float> SumAsync(IQueryable<float> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<float?> SumAsync(IQueryable<float?> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<int> SumAsync<T>(IQueryable<T> source, Expression<Func<T, int>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<int?> SumAsync<T>(IQueryable<T> source, Expression<Func<T, int?>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<long> SumAsync<T>(IQueryable<T> source, Expression<Func<T, long>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<long?> SumAsync<T>(IQueryable<T> source, Expression<Func<T, long?>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<double> SumAsync<T>(IQueryable<T> source, Expression<Func<T, double>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<double?> SumAsync<T>(IQueryable<T> source, Expression<Func<T, double?>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<decimal> SumAsync<T>(IQueryable<T> source, Expression<Func<T, decimal>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<decimal?> SumAsync<T>(IQueryable<T> source, Expression<Func<T, decimal?>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<float> SumAsync<T>(IQueryable<T> source, Expression<Func<T, float>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<float?> SumAsync<T>(IQueryable<T> source, Expression<Func<T, float?>> predicate, CancellationToken cancellationToken = default(CancellationToken));

		Task<T[]> ToArrayAsync<T>(IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken));

		Task<Dictionary<K, T>> ToDictionaryAsync<T, K>(IQueryable<T> source, Func<T, K> keySelector, CancellationToken cancellationToken = default(CancellationToken));

		Task<Dictionary<K, T>> ToDictionaryAsync<T, K>(IQueryable<T> source, Func<T, K> keySelector, IEqualityComparer<K> comparer, CancellationToken cancellationToken = default(CancellationToken));

		Task<Dictionary<K, E>> ToDictionaryAsync<T, K, E>(IQueryable<T> source, Func<T, K> keySelector, Func<T, E> elementSelector, CancellationToken cancellationToken = default(CancellationToken));

		Task<Dictionary<K, E>> ToDictionaryAsync<T, K, E>(IQueryable<T> source, Func<T, K> keySelector, Func<T, E> elementSelector, IEqualityComparer<K> comparer, CancellationToken cancellationToken = default(CancellationToken));

		Task<List<TSource>> ToListAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken));
		#endregion
	}
}

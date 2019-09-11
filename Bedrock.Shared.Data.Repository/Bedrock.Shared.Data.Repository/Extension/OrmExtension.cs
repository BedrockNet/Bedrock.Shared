using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Data.Repository.Interface;
using Bedrock.Shared.Entity.Interface;

using Bedrock.Shared.Pagination;
using Bedrock.Shared.Pagination.Enumeration;

using CommonServiceLocator;

namespace Bedrock.Shared.Data.Repository.Extension
{
	public static class OrmExtension
	{
		#region Constructors
		static OrmExtension()
		{
			OrmHelper = ServiceLocator.Current.GetInstance<IOrmHelper>();
		}
		#endregion

		#region Properties
		private static IOrmHelper OrmHelper { get; set; }
		#endregion

		#region Public Methods
		public static IIncludeQueryable<TEntity, TProperty> Include<TEntity, TProperty>(this IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> path) where TEntity : class where TProperty : class
		{
			return OrmHelper.Include(source, path);
		}

		public static IIncludeQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(this IIncludeQueryable<TEntity, IEnumerable<TPreviousProperty>> query, Expression<Func<TPreviousProperty, TProperty>> path) where TEntity : class
		{
			return OrmHelper.ThenInclude(query, path);
		}

		public static IIncludeQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(this IIncludeQueryable<TEntity, TPreviousProperty> query, Expression<Func<TPreviousProperty, TProperty>> path) where TEntity : class
		{
			return OrmHelper.ThenInclude(query, path);
		}

		public static IQueryable<TEntity> FindBy<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, bool>> predicate)
		{
			return OrmHelper.FindBy(query, predicate);
		}

		public static TEntity GetSingle<TEntity, TKey>(this IQueryable<TEntity> query, TKey id) where TEntity : class, IBedrockEntity, IBedrockIdEntity<TKey> where TKey : IComparable
		{
			return OrmHelper.GetSingle(query, id);
		}
		#endregion

		#region Public Methods (Pagination)
		public static PaginationResult<TEntity> Paginate<TEntity>(this IQueryable<TEntity> query, int pageIndex, int pageSize) where TEntity : class, IBedrockEntity
		{
			return Paginate(query, pageIndex, pageSize, null, SortOrder.Ascending);
		}

		public static PaginationResult<TEntity> Paginate<TEntity>(this IQueryable<TEntity> query, int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortField, SortOrder sortOrder) where TEntity : class
		{
			return Paginate(query, pageIndex, pageSize, sortField, sortOrder, null);
		}

		public static PaginationResult<TEntity> Paginate<TEntity>(this IQueryable<TEntity> query, int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortField, SortOrder sortOrder, Expression<Func<TEntity, bool>> predicate) where TEntity : class
		{
			return OrmHelper.Paginate(query, pageIndex, pageSize, sortField, sortOrder, predicate);
		}

		public static Task<PaginationResult<TEntity>> PaginateAsync<TEntity>(this IQueryable<TEntity> query, int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken)) where TEntity : class, IBedrockEntity
		{
			return PaginateAsync(query, pageIndex, pageSize, null, SortOrder.Ascending, cancellationToken);
		}

		public static Task<PaginationResult<TEntity>> PaginateAsync<TEntity>(this IQueryable<TEntity> query, int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortField, SortOrder sortOrder, CancellationToken cancellationToken = default(CancellationToken)) where TEntity : class
		{
			return PaginateAsync(query, pageIndex, pageSize, sortField, sortOrder, null, cancellationToken);
		}

		public static Task<PaginationResult<TEntity>> PaginateAsync<TEntity>(this IQueryable<TEntity> query, int pageIndex, int pageSize, Expression<Func<TEntity, object>> sortField, SortOrder sortOrder, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) where TEntity : class
		{
			return OrmHelper.PaginateAsync(query, pageIndex, pageSize, sortField, sortOrder, predicate, cancellationToken);
		}

		public static PaginationResult<TEntity> Paginate<TEntity, TSearch>(this IQueryable<TEntity> query, TSearch searchObject, PagingInstruction pagingInstruction, Expression<Func<TEntity, bool>> predicate = null, string prefix = null)
		{
			return OrmHelper.Paginate(query, searchObject, pagingInstruction, predicate, prefix);
		}

		public static Task<PaginationResult<TEntity>> PaginateAsync<TEntity, TSearch>(this IQueryable<TEntity> query, TSearch searchObject, PagingInstruction pagingInstruction, Expression<Func<TEntity, bool>> predicate = null, string prefix = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.PaginateAsync(query, searchObject, pagingInstruction, predicate, prefix, cancellationToken);
		}

		public static PaginationResult<TContract> Paginate<TEntity, TContract, TSearch>(this IQueryable<TEntity> query, TSearch searchObject, PagingInstruction pagingInstruction, Expression<Func<TEntity, bool>> predicate = null, string prefix = null)
		{
			return OrmHelper.Paginate<TEntity, TContract, TSearch>(query, searchObject, pagingInstruction, predicate, prefix);
		}

		public static Task<PaginationResult<TContract>> PaginateAsync<TEntity, TContract, TSearch>(this IQueryable<TEntity> query, TSearch searchObject, PagingInstruction pagingInstruction, Expression<Func<TEntity, bool>> predicate = null, string prefix = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.PaginateAsync<TEntity, TContract, TSearch>(query, searchObject, pagingInstruction, predicate, prefix, cancellationToken);
		}
		#endregion

		#region Public Methods (DynamicQueryable)
		public static bool ParseSearchVars<T>(T searchObject, ref string queryString, ref object[] parameters)
		{
			return OrmHelper.ParseSearchVars(searchObject, ref queryString, ref parameters);
		}

		public static bool ParseSearchVars<T>(T searchObject, string prefix, ref string queryString, ref object[] parameters)
		{
			return OrmHelper.ParseSearchVars(searchObject, prefix, ref queryString, ref parameters);
		}

		public static IQueryable<T> ProcessSearchSettings<T, S>(this IQueryable<T> query, PagingInstruction pager)
		{
			return OrmHelper.ProcessSearchSettings<T, S>(pager, query);
		}

		public static IQueryable<TDatasource> ValidatePaging<TDatasource>(this IQueryable<TDatasource> queryable, PagingInstruction pager) where TDatasource : class
		{
			return OrmHelper.ValidatePaging(pager, queryable);

		}

		public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> query, string predicate, params object[] arguments)
		{
			return OrmHelper.Where(query, predicate, arguments);
		}
		#endregion

		#region Public Methods (Async)
		public static Task<bool> AllAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AllAsync(source, predicate, cancellationToken);
		}

		public static Task<bool> AnyAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AnyAsync(source, cancellationToken);
		}

		public static Task<bool> AnyAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AnyAsync(source, predicate, cancellationToken);
		}

		public static IQueryable<TEntity> AsNoTracking<TEntity>(this IQueryable<TEntity> source) where TEntity : class
		{
			return OrmHelper.AsNoTracking(source);
		}

		public static IQueryable<TEntity> AsTracking<TEntity>(this IQueryable<TEntity> source) where TEntity : class
		{
			return OrmHelper.AsTracking(source);
		}

		public static Task<double> AverageAsync(this IQueryable<int> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, cancellationToken);
		}

		public static Task<double?> AverageAsync(this IQueryable<int?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, cancellationToken);
		}

		public static Task<double> AverageAsync(this IQueryable<long> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, cancellationToken);
		}

		public static Task<double?> AverageAsync(this IQueryable<long?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, cancellationToken);
		}

		public static Task<double> AverageAsync(this IQueryable<double> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, cancellationToken);
		}

		public static Task<double?> AverageAsync(this IQueryable<double?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, cancellationToken);
		}

		public static Task<decimal> AverageAsync(this IQueryable<decimal> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, cancellationToken);
		}

		public static Task<decimal?> AverageAsync(this IQueryable<decimal?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, cancellationToken);
		}

		public static Task<float> AverageAsync(this IQueryable<float> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, cancellationToken);
		}

		public static Task<float?> AverageAsync(this IQueryable<float?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, cancellationToken);
		}

		public static Task<double> AverageAsync<T>(this IQueryable<T> source, Expression<Func<T, int>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, predicate, cancellationToken);
		}

		public static Task<double?> AverageAsync<T>(this IQueryable<T> source, Expression<Func<T, int?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, predicate, cancellationToken);
		}

		public static Task<double> AverageAsync<T>(this IQueryable<T> source, Expression<Func<T, long>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, predicate, cancellationToken);
		}

		public static Task<double?> AverageAsync<T>(this IQueryable<T> source, Expression<Func<T, long?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, predicate, cancellationToken);
		}

		public static Task<double> AverageAsync<T>(this IQueryable<T> source, Expression<Func<T, double>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, predicate, cancellationToken);
		}

		public static Task<double?> AverageAsync<T>(this IQueryable<T> source, Expression<Func<T, double?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, predicate, cancellationToken);
		}

		public static Task<decimal> AverageAsync<T>(this IQueryable<T> source, Expression<Func<T, decimal>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, predicate, cancellationToken);
		}

		public static Task<decimal?> AverageAsync<T>(this IQueryable<T> source, Expression<Func<T, decimal?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, predicate, cancellationToken);
		}

		public static Task<float> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, predicate, cancellationToken);
		}

		public static Task<float?> AverageAsync<T>(this IQueryable<T> source, Expression<Func<T, float?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.AverageAsync(source, predicate, cancellationToken);
		}

		public static Task<bool> ContainsAsync<T>(this IQueryable<T> source, T item, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.ContainsAsync(source, item, cancellationToken);
		}

		public static Task<int> CountAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.CountAsync(source, cancellationToken);
		}

		public static Task<int> CountAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.CountAsync(source, predicate, cancellationToken);
		}

		public static Task<T> FirstAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.FirstAsync(source, cancellationToken);
		}

		public static Task<T> FirstAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.FirstAsync(source, predicate, cancellationToken);
		}

		public static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.FirstOrDefaultAsync(source, cancellationToken);
		}

		public static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.FirstOrDefaultAsync(source, predicate, cancellationToken);
		}

		public static Task ForEachAsync<TSource>(this IQueryable<TSource> source, Action<TSource> action, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.ForEachAsync(source, action, cancellationToken);
		}

		public static Task<TSource> LastAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.LastAsync(source, cancellationToken);
		}

		public static Task<TSource> LastAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.LastAsync(source, predicate, cancellationToken);
		}

		public static Task<TSource> LastOrDefaultAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.LastOrDefaultAsync(source, cancellationToken);
		}

		public static Task<TSource> LastOrDefaultAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.LastOrDefaultAsync(source, predicate, cancellationToken);
		}

		public static void Load<TSource>(this IQueryable<TSource> source)
		{
			OrmHelper.Load(source);
		}

		public static Task LoadAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.LoadAsync(source, cancellationToken);
		}

		public static Task<long> LongCountAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.LongCountAsync(source, cancellationToken);
		}

		public static Task<long> LongCountAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.LongCountAsync(source, predicate, cancellationToken);
		}

		public static Task<T> MaxAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.MaxAsync(source, cancellationToken);
		}

		public static Task<T> MaxAsync<T, R>(this IQueryable<T> source, Expression<Func<T, R>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.MaxAsync(source, predicate, cancellationToken);
		}

		public static Task<T> MinAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.MinAsync(source, cancellationToken);
		}

		public static Task<T> MinAsync<T, R>(this IQueryable<T> source, Expression<Func<T, R>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.MinAsync(source, predicate, cancellationToken);
		}

		public static Task<T> SingleAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SingleAsync(source, cancellationToken);
		}

		public static Task<T> SingleAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SingleAsync(source, predicate, cancellationToken);
		}

		public static Task<T> SingleOrDefaultAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SingleOrDefaultAsync(source, cancellationToken);
		}

		public static Task<T> SingleOrDefaultAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SingleOrDefaultAsync(source, predicate, cancellationToken);
		}

		public static Task<int> SumAsync(this IQueryable<int> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, cancellationToken);
		}

		public static Task<int?> SumAsync(this IQueryable<int?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, cancellationToken);
		}

		public static Task<long> SumAsync(this IQueryable<long> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, cancellationToken);
		}

		public static Task<long?> SumAsync(this IQueryable<long?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, cancellationToken);
		}

		public static Task<double> SumAsync(this IQueryable<double> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, cancellationToken);
		}

		public static Task<double?> SumAsync(this IQueryable<double?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, cancellationToken);
		}

		public static Task<decimal> SumAsync(this IQueryable<decimal> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, cancellationToken);
		}

		public static Task<decimal?> SumAsync(this IQueryable<decimal?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, cancellationToken);
		}

		public static Task<float> SumAsync(this IQueryable<float> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, cancellationToken);
		}

		public static Task<float?> SumAsync(this IQueryable<float?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, cancellationToken);
		}

		public static Task<int> SumAsync<T>(this IQueryable<T> source, Expression<Func<T, int>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, predicate, cancellationToken);
		}

		public static Task<int?> SumAsync<T>(this IQueryable<T> source, Expression<Func<T, int?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, predicate, cancellationToken);
		}

		public static Task<long> SumAsync<T>(this IQueryable<T> source, Expression<Func<T, long>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, predicate, cancellationToken);
		}

		public static Task<long?> SumAsync<T>(this IQueryable<T> source, Expression<Func<T, long?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, predicate, cancellationToken);
		}

		public static Task<double> SumAsync<T>(this IQueryable<T> source, Expression<Func<T, double>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, predicate, cancellationToken);
		}

		public static Task<double?> SumAsync<T>(this IQueryable<T> source, Expression<Func<T, double?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, predicate, cancellationToken);
		}

		public static Task<decimal> SumAsync<T>(this IQueryable<T> source, Expression<Func<T, decimal>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, predicate, cancellationToken);
		}

		public static Task<decimal?> SumAsync<T>(this IQueryable<T> source, Expression<Func<T, decimal?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, predicate, cancellationToken);
		}

		public static Task<float> SumAsync<T>(this IQueryable<T> source, Expression<Func<T, float>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, predicate, cancellationToken);
		}

		public static Task<float?> SumAsync<T>(this IQueryable<T> source, Expression<Func<T, float?>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.SumAsync(source, predicate, cancellationToken);
		}

		public static Task<T[]> ToArrayAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.ToArrayAsync(source, cancellationToken);
		}

		public static Task<Dictionary<K, T>> ToDictionaryAsync<T, K>(this IQueryable<T> source, Func<T, K> keySelector, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.ToDictionaryAsync(source, keySelector, cancellationToken);
		}

		public static Task<Dictionary<K, T>> ToDictionaryAsync<T, K>(this IQueryable<T> source, Func<T, K> keySelector, IEqualityComparer<K> comparer, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.ToDictionaryAsync(source, keySelector, comparer, cancellationToken);
		}

		public static Task<Dictionary<K, E>> ToDictionaryAsync<T, K, E>(this IQueryable<T> source, Func<T, K> keySelector, Func<T, E> elementSelector, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.ToDictionaryAsync(source, keySelector, elementSelector, cancellationToken);
		}

		public static Task<Dictionary<K, E>> ToDictionaryAsync<T, K, E>(this IQueryable<T> source, Func<T, K> keySelector, Func<T, E> elementSelector, IEqualityComparer<K> comparer, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.ToDictionaryAsync(source, keySelector, elementSelector, comparer, cancellationToken);
		}

		public static Task<List<TSource>> ToListAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			return OrmHelper.ToListAsync(source, cancellationToken);
		}
		#endregion
	}
}

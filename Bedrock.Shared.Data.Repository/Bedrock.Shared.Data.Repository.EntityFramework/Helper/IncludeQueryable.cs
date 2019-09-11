using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Bedrock.Shared.Data.Repository.Interface;

namespace Bedrock.Shared.Data.Repository.EntityFramework.Helper
{
	public class IncludeQueryable<TEntity, TProperty> : IIncludeQueryable<TEntity, TProperty>, IAsyncEnumerable<TEntity>
		where TEntity : class
	{
		#region Constructors
		public IncludeQueryable(IQueryable<TEntity> queryable)
		{
			Queryable = queryable;
		}
		#endregion

		#region Properties
		public IQueryable<TEntity> Queryable { get; private set; }

		public Expression Expression => Queryable.Expression;

		public Type ElementType => Queryable.ElementType;

		public IQueryProvider Provider => Queryable.Provider;

		public IEnumerator<TEntity> GetEnumerator() => Queryable.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		IAsyncEnumerator<TEntity> IAsyncEnumerable<TEntity>.GetEnumerator() => ((IAsyncEnumerable<TEntity>)Queryable).GetEnumerator();
		#endregion
	}
}

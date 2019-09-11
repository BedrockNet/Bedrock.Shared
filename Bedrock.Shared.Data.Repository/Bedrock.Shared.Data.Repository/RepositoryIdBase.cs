using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Bedrock.Shared.Configuration;
using Bedrock.Shared.Data.Repository.Interface;
using Bedrock.Shared.Entity.Interface;
using Bedrock.Shared.Session.Interface;

namespace Bedrock.Shared.Data.Repository
{
	public abstract class RepositoryIdBase<TEntity, TContext, TKey> : RepositoryBase<TEntity, TContext>,
																		IRepositoryId<TEntity, TKey>,
																		IRepositoryOrmId<TEntity, TKey>,
																		IAsyncEnumerable<TEntity>
		where TEntity : class, IBedrockEntity
		where TContext : IUnitOfWork
		where TKey : IComparable
	{
		#region Constructors
		public RepositoryIdBase(BedrockConfiguration bedrockConfiguration) : base(bedrockConfiguration) { }
		#endregion

		#region IQueryable<T> Properties
		Type IQueryable.ElementType
		{
			get { return RootQueryable.ElementType; }
		}

		Expression IQueryable.Expression
		{
			get { return RootQueryable.Expression; }
		}

		IQueryProvider IQueryable.Provider
		{
			get { return RootQueryable.Provider; }
		}
		#endregion

		#region IRepositoryId<TEntity> Methods
		public abstract TEntity GetSingle(TKey id);
		#endregion

		#region IQueryable<T> Methods
		IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
		{
			return RootQueryable.GetEnumerator();
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return RootQueryable.GetEnumerator();
		}
		#endregion

		#region IAsyncEnumerable<T> Methods
		IAsyncEnumerator<TEntity> IAsyncEnumerable<TEntity>.GetEnumerator()	=> ((IAsyncEnumerable<TEntity>)RootQueryable).GetEnumerator();
		#endregion
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Configuration;
using Bedrock.Shared.Data.Repository.Interface;

using Bedrock.Shared.Entity.Enumeration;
using Bedrock.Shared.Entity.Interface;

using Bedrock.Shared.Session.Implementation;
using Bedrock.Shared.Session.Interface;

namespace Bedrock.Shared.Data.Repository
{
	public abstract class RepositoryBase<TEntity, TContext> : ContextBase<TContext>,
																	IRepository<TEntity>,
																	IRepositoryOrm<TEntity>,
																	IAsyncEnumerable<TEntity>
		where TEntity : class, IBedrockEntity
		where TContext : IUnitOfWork
	{
		#region Constructors
		public RepositoryBase(BedrockConfiguration bedrockConfiguration)
		{
            BedrockConfiguration = bedrockConfiguration;
		}
		#endregion

		#region IRepositoryOrm Properties
		public abstract IQueryable<TEntity> RootQueryable { get; }
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

		#region Protected Properties
		protected BedrockConfiguration BedrockConfiguration { get; set; }
		#endregion

		#region IRepository<TEntity> Methods
		public abstract void Add(TEntity entity);

		public abstract void Update(TEntity entity);

		public abstract void Remove(TEntity entity);

		public abstract void RemoveRange(params TEntity[] entities);

		public abstract void RemoveRange(IEnumerable<TEntity> entities);

		public abstract TEntity Find(params object[] keyValues);

		public abstract IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

		public abstract Task<TEntity> FindAsync(params object[] keyValues);

		public abstract Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
		#endregion

		#region IRepositoryOrm<TEntity> Methods
		public abstract void Attach(TEntity entity);

		public abstract IIncludeQueryable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> path) where TProperty : class;

		public abstract EntityState GetState(TEntity entity);

		public abstract void SetState(TEntity entity, EntityState entityState);

		public abstract IEnumerable<TEntity> WhereAdded(Expression<Func<TEntity, bool>> predicate);

		public abstract int ExecuteNonQuery(string commandText, SqlParameter[] parameters);

		public abstract Task<int> ExecuteNonQueryAsync(string commandText, SqlParameter[] parameters, CancellationToken cancellationToken);

		public abstract IQueryable<TEntity> ExecuteQuery(string commandText, SqlParameter[] parameters);

		public abstract IEnumerable<T> ExecuteQuery<T>(string commandText, SqlParameter[] parameters) where T : new();

		public abstract Task<IEnumerable<T>> ExecuteQueryAsync<T>(string commandText, SqlParameter[] parameters, CancellationToken cancellationToken) where T : new();
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

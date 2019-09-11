using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Entity.Enumeration;
using Bedrock.Shared.Entity.Interface;

namespace Bedrock.Shared.Data.Repository.Interface
{
	public interface IRepositoryOrm<TEntity> : IRepository<TEntity>
		where TEntity : class, IBedrockEntity
	{
		#region Properties
		IQueryable<TEntity> RootQueryable { get; }
		#endregion

		#region Methods
		void Attach(TEntity entity);

		IIncludeQueryable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> path) where TProperty : class;

		EntityState GetState(TEntity entity);

		void SetState(TEntity entity, EntityState entityState);

		IEnumerable<TEntity> WhereAdded(Expression<Func<TEntity, bool>> predicate);

		int ExecuteNonQuery(string commandText, SqlParameter[] parameters);

		Task<int> ExecuteNonQueryAsync(string commandText, SqlParameter[] parameters, CancellationToken cancellationToken = default(CancellationToken));

		IQueryable<TEntity> ExecuteQuery(string commandText, SqlParameter[] parameters);

		IEnumerable<T> ExecuteQuery<T>(string commandText, SqlParameter[] parameters) where T : new();

		Task<IEnumerable<T>> ExecuteQueryAsync<T>(string commandText, SqlParameter[] parameters, CancellationToken cancellationToken = default(CancellationToken)) where T : new();
		#endregion
	}
}

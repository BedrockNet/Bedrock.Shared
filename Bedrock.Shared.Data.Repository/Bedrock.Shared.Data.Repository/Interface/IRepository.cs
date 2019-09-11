using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Entity.Interface;
using Bedrock.Shared.Session.Interface;

namespace Bedrock.Shared.Data.Repository.Interface
{
	public interface IRepository<TEntity> : ISessionAware, IQueryable<TEntity>
		where TEntity : class, IBedrockEntity
	{
		#region Methods
		void Add(TEntity entity);

		void Update(TEntity entity);

		void Remove(TEntity entity);

		void RemoveRange(params TEntity[] entities);

		void RemoveRange(IEnumerable<TEntity> entities);

		TEntity Find(params object[] keyValues);

		IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

		Task<TEntity> FindAsync(params object[] keyValues);

		Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
		#endregion
	}
}

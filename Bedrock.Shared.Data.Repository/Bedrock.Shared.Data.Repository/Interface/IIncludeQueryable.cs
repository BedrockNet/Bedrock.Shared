using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bedrock.Shared.Data.Repository.Interface
{
	public interface IIncludeQueryable<out TEntity, out TProperty> : IQueryable<TEntity>, IEnumerable<TEntity>, IEnumerable, IQueryable
		where TEntity : class
	{
		#region Properties
		IQueryable<TEntity> Queryable { get; }
		#endregion
	}
}

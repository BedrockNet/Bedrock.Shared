using System;
using Bedrock.Shared.Entity.Interface;

namespace Bedrock.Shared.Data.Repository.Interface
{
	public interface IRepositoryId<TEntity, TKey> : IRepository<TEntity>
		where TEntity : class, IBedrockEntity
		where TKey : IComparable
	{
		#region Methods
		TEntity GetSingle(TKey id);
		#endregion
	}
}

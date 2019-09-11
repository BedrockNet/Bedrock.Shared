using System;
using Bedrock.Shared.Entity.Interface;

namespace Bedrock.Shared.Data.Repository.Interface
{
	public interface IRepositoryOrmId<TEntity, TKey> : IRepositoryOrm<TEntity>, IRepositoryId<TEntity, TKey>
	   where TEntity : class, IBedrockEntity
	   where TKey : IComparable
	{ }
}

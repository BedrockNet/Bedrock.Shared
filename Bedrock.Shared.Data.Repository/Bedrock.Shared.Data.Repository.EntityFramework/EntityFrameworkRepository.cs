using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Configuration;

using Bedrock.Shared.Data.Repository.EntityFramework.Helper;
using Bedrock.Shared.Data.Repository.Extension;
using Bedrock.Shared.Data.Repository.Interface;

using Bedrock.Shared.Entity.Interface;
using Bedrock.Shared.Extension;
using Bedrock.Shared.Session.Interface;

using Microsoft.EntityFrameworkCore;
using SharedEntityEnumeration = Bedrock.Shared.Entity.Enumeration;

namespace Bedrock.Shared.Data.Repository.EntityFramework
{
    public class EntityFrameworkRepository<TEntity, TContext> : RepositoryBase<TEntity, TContext>,
                                                                    IRepository<TEntity>,
                                                                    IRepositoryOrm<TEntity>
        where TEntity : class, IBedrockEntity
        where TContext : DbContext, IUnitOfWork
    {
		#region Constructors
		public EntityFrameworkRepository(BedrockConfiguration bedrockConfiguration) : base(bedrockConfiguration) { }
		#endregion

		#region IRepositoryOrm Properties
		public override IQueryable<TEntity> RootQueryable { get { return DbSet.OfType<TEntity>(); } }
        #endregion

        #region Properties
        protected virtual DbSet<TEntity> DbSet { get { return Context.Set<TEntity>(); } }

        protected IOrmHelper OrmHelper => new OrmHelper();
        #endregion

        #region IRepository<TEntity> Methods
        public override void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public override void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public override void Remove(TEntity entity)
        {
			if (!BedrockConfiguration.Data.IsSoftDelete)
				DbSet.Remove(entity);
			else
			{
				var deletableEntity = entity as IBedrockDeletableEntity;
				deletableEntity.IsDeleted = true;

				Context.Entry(entity).State = EntityState.Modified;
			}
		}

        public override void RemoveRange(params TEntity[] entities)
        {
			if (!BedrockConfiguration.Data.IsSoftDelete)
				DbSet.RemoveRange(entities);
			else
			{
				entities.Each(e =>
				{
					var deletableEntity = e as IBedrockDeletableEntity;
					deletableEntity.IsDeleted = true;

					Context.Entry(e).State = EntityState.Modified;
				});
			}
        }

        public override void RemoveRange(IEnumerable<TEntity> entities)
        {
			if (!BedrockConfiguration.Data.IsSoftDelete)
				DbSet.RemoveRange(entities);
			else
			{
				entities.Each(e =>
				{
					var deletableEntity = e as IBedrockDeletableEntity;
					deletableEntity.IsDeleted = true;

					Context.Entry(e).State = EntityState.Modified;
				});
			}
		}

        public override TEntity Find(params object[] keyValues)
        {
            return DbSet.Find(keyValues);
        }

		public override IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
		{
			return DbSet.FindBy(predicate);
		}

		public override Task<TEntity> FindAsync(params object[] keyValues)
		{
			return DbSet.FindAsync(keyValues);
		}

		public override Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
		{
			return DbSet.FindAsync(keyValues, cancellationToken);
		}
		#endregion

		#region IRepositoryOrm<TEntity> Methods
		public override void Attach(TEntity entity)
		{
			DbSet.Attach(entity);
		}

		public override IIncludeQueryable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> path)
        {
            return OrmHelper.Include(DbSet, path);
        }

        public override SharedEntityEnumeration.EntityState GetState(TEntity entity)
        {
            var entityState = Context.Entry(entity).State;
            return EntityHelper.ConvertStateForGet(entityState);
        }

        public override void SetState(TEntity entity, SharedEntityEnumeration.EntityState entityState)
        {
            Context.Entry(entity).State = EntityHelper.ConvertStateForSet(entityState);
        }

        public override IEnumerable<TEntity> WhereAdded(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Local.Where(predicate.Compile());
        }

        public override int ExecuteNonQuery(string commandText, SqlParameter[] parameters)
        {
            var arguments = parameters.PrepareArguments(commandText);
            return Context.Database.ExecuteSqlCommand(arguments.Item1, arguments.Item2);
        }

        public override async Task<int> ExecuteNonQueryAsync(string commandText, SqlParameter[] parameters, CancellationToken cancellationToken)
        {
            var arguments = parameters.PrepareArguments(commandText);
            return await Context.Database.ExecuteSqlCommandAsync(arguments.Item1, cancellationToken, arguments.Item2);
        }

        public override IQueryable<TEntity> ExecuteQuery(string commandText, SqlParameter[] parameters)
        {
            var arguments = parameters.PrepareArguments(commandText);
            return this.FromSql(arguments.Item1, arguments.Item2);
        }

        public override IEnumerable<T> ExecuteQuery<T>(string commandText, SqlParameter[] parameters)
        {
            var arguments = parameters.PrepareArguments(commandText);
            return Context.Database.GetModelFromQuery<T>(arguments.Item1, arguments.Item2);
        }

        public override async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string commandText, SqlParameter[] parameters, CancellationToken cancellationToken = default(CancellationToken))
        {
            var arguments = parameters.PrepareArguments(commandText);
            return await Context.Database.GetModelFromQueryAsync<T>(arguments.Item1, cancellationToken, arguments.Item2);
        }
        #endregion
    }
}

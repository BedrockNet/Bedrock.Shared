using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Bedrock.Shared.Domain.Interface;
using Bedrock.Shared.Entity.Interface;
using Bedrock.Shared.Security.Interface;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using EntityEnumeration = Bedrock.Shared.Entity.Enumeration;

namespace Bedrock.Shared.Data.Repository.EntityFramework.Helper
{
	public static class EntityHelper
	{
		#region Public Methods
		public static IEnumerable<IBedrockDomainEventEntity> GetEventEntries(DbContext context)
		{
			return context
					.ChangeTracker
					.Entries()
					.Where(ee => typeof(IBedrockDomainEventEntity).IsAssignableFrom(ee.Entity.GetType()) && ee.Entity != null)
					.Select(ee => (IBedrockDomainEventEntity)ee.Entity)
					.Where(ee => ee.Events.Any())
					.ToArray();
		}

		public static IEnumerable<EntityEntry> GetChangedEntries(DbContext context)
		{
			return context
					.ChangeTracker
					.Entries()
					.Where(ee => typeof(IBedrockEntity).IsAssignableFrom(ee.Entity.GetType()) && ee.Entity != null
									&& (ee.State == EntityState.Added || ee.State == EntityState.Modified))
					.Select(ee => ee);
		}

		public static void EnsureEntityModified(EntityEntry entry)
		{
			if (entry.State != EntityState.Modified)
				return;

			var isChanged = false;

			for (int i = 0; i < entry.OriginalValues.Properties.Count() && !isChanged; ++i)
			{
				var propertyName = entry.OriginalValues.Properties[i].Name;
				var originalValue = entry.Property(propertyName).OriginalValue;
				var currentValue = entry.Property(propertyName).CurrentValue;

				if ((originalValue == null || currentValue == null) || (!originalValue.Equals(currentValue) && (!(originalValue is byte[]) || !((byte[])originalValue).SequenceEqual((byte[])currentValue))))
				{
					isChanged = true;
					break;
				}
			}

			if (!isChanged)
				entry.State = EntityState.Unchanged;
		}

		public static void SetRowAuditDataEntity(EntityEntry entry, IBedrockUser user, DateTime currentDateTime)
		{
			var auditableEntity = entry.Entity as IBedrockAuditEntity;

			if (auditableEntity != null)
			{
				if (entry.State == EntityState.Added)
				{
					auditableEntity.CreatedBy = user.UserId;
					auditableEntity.CreatedDate = currentDateTime;

                    auditableEntity.UpdatedBy = user.UserId;
                    auditableEntity.UpdatedDate = currentDateTime;
                }
                else if(entry.State == EntityState.Modified)
                {
                    auditableEntity.UpdatedBy = user.UserId;
                    auditableEntity.UpdatedDate = currentDateTime;
                }
			}
		}

		public static void SetRowAuditDataEntityDeletable(EntityEntry entry, IBedrockUser user, DateTime currentDateTime)
		{
			var deletableEntity = entry.Entity as IBedrockDeletableEntity;

			if (deletableEntity != null && deletableEntity.IsDeleted)
			{
				deletableEntity.DeletedBy = user.UserId;
				deletableEntity.DeletedDate = currentDateTime;
			}
		}

		public static EntityEnumeration.EntityState ConvertStateForGet(EntityState entityState)
		{
			switch (entityState)
			{
				case EntityState.Added:
					return EntityEnumeration.EntityState.Added;
				case EntityState.Deleted:
					return EntityEnumeration.EntityState.Deleted;
				case EntityState.Modified:
					return EntityEnumeration.EntityState.Modified;
				case EntityState.Detached:
					return EntityEnumeration.EntityState.Detached;
				default:
					return EntityEnumeration.EntityState.Unchanged;
			}
		}

		public static EntityState ConvertStateForSet(EntityEnumeration.EntityState entityState)
		{
			switch (entityState)
			{
				case EntityEnumeration.EntityState.Added:
					return EntityState.Added;
				case EntityEnumeration.EntityState.Deleted:
					return EntityState.Deleted;
				case EntityEnumeration.EntityState.Modified:
					return EntityState.Modified;
				case EntityEnumeration.EntityState.Detached:
					return EntityState.Detached;
				default:
					return EntityState.Unchanged;
			}
		}

		public static IQueryable<TEntity> Filter<TEntity, TProperty>(IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> property, TProperty value) where TProperty : IComparable
		{
			var memberExpression = property.Body as MemberExpression;

			if (memberExpression == null || !(memberExpression.Member is PropertyInfo))
				throw new ArgumentException("Property expected", "property");

			var left = property.Body;
			var right = Expression.Constant(value, typeof(TProperty));
			var searchExpression = Expression.Equal(left, right);
			var lambda = Expression.Lambda<Func<TEntity, bool>>(searchExpression, new ParameterExpression[] { property.Parameters.Single() });

			return query.Where(lambda);
		}
		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Bedrock.Shared.Configuration;
using Bedrock.Shared.Data.Repository.EntityFramework.Helper;
using Bedrock.Shared.Domain.Interface;
using Bedrock.Shared.Entity.Interface;
using Bedrock.Shared.Extension;

using Bedrock.Shared.Security.Interface;
using Bedrock.Shared.Security.Model;
using Bedrock.Shared.Service.Interface;
using Bedrock.Shared.Session.Interface;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bedrock.Shared.Data.Repository.EntityFramework
{
    public class BedrockContext : EntityFrameworkDataContext, IUnitOfWork
	{
		#region Fields
		private ISession _session;
		#endregion

		#region Constructors
		public BedrockContext(IDomainEventDispatcher domainEventDispatcher, BedrockConfiguration bedrockConfiguration) : base(bedrockConfiguration)
		{
			Initialize(domainEventDispatcher);
		}

        public BedrockContext(string connectionString, IDomainEventDispatcher domainEventDispatcher, BedrockConfiguration bedrockConfiguration) : base(connectionString, bedrockConfiguration)
		{
			Initialize(domainEventDispatcher);
		}

        public BedrockContext(DbContextOptions options, IDomainEventDispatcher domainEventDispatcher, BedrockConfiguration bedrockConfiguration) : base(options, bedrockConfiguration)
		{
			Initialize(domainEventDispatcher);
		}
		#endregion

		#region IUnitOfWork Properties
		public ISession Session
		{
			get { return _session; }
			set
			{
				_session = value;
				DomainEventDispatcher.Enlist(_session);
			}
		}
		#endregion

		#region Protected Properties
		protected IDomainEventDispatcher DomainEventDispatcher { get; private set; }
		#endregion

		#region DbContext Members
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			ConfigureSoftDeleteOnModelCreating(modelBuilder);
			base.OnModelCreating(modelBuilder);
		}

		public override int SaveChanges()
        {
            SaveChangesInternal();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            SaveChangesInternal();
            return await base.SaveChangesAsync();
        }
        #endregion

        #region IUnitOfWork Members
        public override int SaveChanges<T, C>(IServiceResponse<T, C> response)
        {
            SaveChangesInternal();
            var returnValue = base.SaveChanges();

            PostSave(response);

            return returnValue;
        }

        public override async Task<int> SaveChangesAsync<T, C>(IServiceResponse<T, C> response)
        {
            SaveChangesInternal();
            var returnValue = await base.SaveChangesAsync();

            PostSave(response);

            return returnValue;
        }
		#endregion

		#region Protected Methods
		protected void DispatchDomainEvents(IEnumerable<IBedrockDomainEventEntity> domainEventEntities)
		{
			domainEventEntities.Each(ee =>
			{
				if (ee.Events.Any())
				{
					var events = ee.Events.ToList();
					ee.ClearEvents();

					events.Each(e => DomainEventDispatcher.Dispatch(e));
				}
			});
		}

		protected void EnsureEntitiesModified(IEnumerable<EntityEntry> changedEntries)
        {
            if (!BedrockConfiguration.Data.IsEnsureEntitiesModified)
                return;

            changedEntries.Each(e => EntityHelper.EnsureEntityModified(e));
        }

		protected void SetRowAuditDataEntities(IEnumerable<EntityEntry> changedEntries, IBedrockUser user, DateTime currentDateTime)
		{
			if (!BedrockConfiguration.Data.AutoSaveAuditFields)
				return;

			changedEntries.Each(e => EntityHelper.SetRowAuditDataEntity(e, user, currentDateTime));
		}

		protected void SetRowAuditDataEntitiesDeletable(IEnumerable<EntityEntry> changedEntries, IBedrockUser user, DateTime currentDateTime)
		{
			if (!BedrockConfiguration.Data.AutoSaveAuditFields)
				return;

			changedEntries.Each(e => EntityHelper.SetRowAuditDataEntityDeletable(e, user, currentDateTime));
		}

		protected void ConfigureSoftDeleteOnModelCreating(ModelBuilder modelBuilder)
		{
			if (BedrockConfiguration.Data.IsSoftDelete)
			{
				modelBuilder
					.Model
					.GetEntityTypes()
					.Where(entityType => typeof(IBedrockDeletableEntity).IsAssignableFrom(entityType.ClrType))
					.ToList()
					.Each(et =>
					{
						typeof(BedrockContext)
							.GetMethod(nameof(ConfigureSoftDelete), BindingFlags.NonPublic | BindingFlags.Instance)
							.MakeGenericMethod(et.ClrType)
							.Invoke(this, new object[] { modelBuilder });
					});
			}
		}
		#endregion

		#region Private Methods
		private void SaveChangesInternal()
        {
			var bedrockUser = Session.User ?? BedrockUser.Default;
			var currentDateTime = DateTime.Now;
			var eventEntities = EntityHelper.GetEventEntries(this);
			var changedEntities = EntityHelper.GetChangedEntries(this);

			DispatchDomainEvents(eventEntities);
			SetRowAuditDataEntitiesDeletable(changedEntities, bedrockUser, currentDateTime);
			EnsureEntitiesModified(changedEntities);
			SetRowAuditDataEntities(changedEntities, bedrockUser, currentDateTime);
        }

		private void Initialize(IDomainEventDispatcher domainEventDispatcher)
		{
			DomainEventDispatcher = domainEventDispatcher;
		}

		protected override void Initialize()
        {
            // TODO:  Add IObjectWithState hook here
            // Core set of EF hooks (i.e., ObjectMaterialized) slated to be added back in EF 2.0

            base.Initialize();
        }

        private void PostSave<T, C>(IServiceResponse<T, C> response)
            where T : class
            where C : class
        {
            response.PostSave();
        }

		private void ConfigureSoftDelete<T>(ModelBuilder builder) where T : class, IBedrockDeletableEntity
		{
			builder.Entity<T>().HasQueryFilter(e => EF.Property<DateTime?>(e, "DeletedDate") == null);
		}
		#endregion
	}
}

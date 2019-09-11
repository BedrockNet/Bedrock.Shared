using System.Collections.Generic;

namespace Bedrock.Shared.Configuration
{
	public class Data
	{
		#region Properties
		public int CommandTimeout { get; set; }

		public bool AutoDetectChangesEnabled { get; set; }

		public bool LazyLoadingEnabled { get; set; }

		public bool ProxyCreationEnabled { get; set; }

		public bool ValidateOnSaveEnabled { get; set; }

		public bool IsValidateOnPropertyChanged { get; set; }

		public bool AutoSaveAuditFields { get; set; }

		public bool IsEnsureEntitiesModified { get; set; }

		public bool IsReadUncommitted { get; set; }

		public bool IsSoftDelete { get; set; }

		public string SoftDeleteColumn { get; set; }

		public bool TruncateDecimalsToScale { get; set; }

		public bool IsLog { get; set; }

		public bool IsAutoTransactionsEnabled { get; set; }

		public bool IsUseRowNumberForPaging { get; set; }

		public Tracking Tracking { get; set; }

        public Dictionary<string, string> ConnectionStrings { get; set; }
        #endregion
    }

	public class Tracking
	{
		#region Public Properties
		public bool IsStoreOriginalValues { get; set; }

		public bool IsSelfTrackingEntitiesEnabled { get; set; }

		public bool IsChangeTrackerEnabled { get; set; }
		#endregion
	}
}

using System.Collections.Generic;
using Bedrock.Shared.Entity.Enumeration;

namespace Bedrock.Shared.Entity.Interface
{
    public interface IObjectWithState
    {
        #region Properties
        EntityState EntityState { get; set; }

        Dictionary<string, object> OriginalValues { get; set; }

        List<string> ModifiedProperties { get; set; }
        #endregion

        #region Methods
        void SetStateAdded();

        void SetStateDeleted();

        void SetStateModified();

        void SetStateDetached();

        void SetStateUnchanged();
        #endregion
    }
}

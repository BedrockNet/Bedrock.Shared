using System;
using System.Collections;
using System.Collections.Specialized;

namespace Bedrock.Shared.Web.Client.Configuration
{
    public class ConfigurationChangedEventArgs : EventArgs
    {
        #region Constructor
        public ConfigurationChangedEventArgs
        (
            string propertyChanged,
            bool isList = false,
            IList oldItems = null,
            IList newItems = null,
            NotifyCollectionChangedAction listAction = NotifyCollectionChangedAction.Add
        )
        {
            IsList = isList;
            PropertyChanged = propertyChanged;

            if (oldItems != null)
                OldItems = new ArrayList(oldItems);

            if (newItems != null)
                NewItems = new ArrayList(newItems);

            ListAction = listAction;
        }
        #endregion

        #region Public Properties
        public bool IsList { get; set; }

        public string PropertyChanged { get; set; }

        public ArrayList OldItems { get; set; }

        public ArrayList NewItems { get; set; }

        public NotifyCollectionChangedAction ListAction { get; set; }
        #endregion
    }
}

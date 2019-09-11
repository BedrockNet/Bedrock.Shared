using System.Collections.Generic;

namespace Bedrock.Shared.Queue
{
    public class QueueSettings
    {
        #region Constructors
        public QueueSettings()
        {
            MessageAttributeNames = new List<string>();
        }
        #endregion

        #region Public Properties
        public string QueueNameOrUri { get; set; }

        public string QueueServer { get; set; }

        public List<string> MessageAttributeNames { get; set; }
        #endregion
    }
}

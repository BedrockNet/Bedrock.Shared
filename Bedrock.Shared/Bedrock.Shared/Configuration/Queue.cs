using System;
using System.Collections.Generic;

using Bedrock.Shared.Enumeration;

namespace Bedrock.Shared.Configuration
{
	public class Queue
	{
		#region Fields
		private List<QueueType> _configuredQueues;
		#endregion

		#region Properties
		public QueueType QueueType { get; set; }

		public List<QueueType> ConfiguredQueues
		{
			get
			{
				_configuredQueues = _configuredQueues ?? new List<QueueType>();
				return _configuredQueues;
			}
			set { _configuredQueues = value; }
		}

		public Msmq Msmq { get; set; }
		#endregion
	}

	public class Msmq
	{
		#region Properties
		public string QueueServer { get; set; }

		public string PrivatePath { get; set; }

		public bool IsPrivatePath { get; set; }

		public bool IsTransactional { get; set; }

		public MessageFormatter MessageFormatter { get; set; }

		public TimeSpan ReceiveTimeout { get; set; }
		#endregion
	}
}

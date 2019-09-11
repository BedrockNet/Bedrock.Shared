using System.Collections.Generic;
using Bedrock.Shared.Enumeration;

namespace Bedrock.Shared.Configuration
{
	public class DistributedService
	{
		#region Fields
		private List<Service> _services;
		#endregion

		#region Properties
		public List<Service> Services
		{
			get
			{
				_services = _services ?? new List<Service>();
				return _services;
			}
			set { _services = value; }
		}
		#endregion
	}

	public class Service
	{
		#region Properties
		public string Name { get; set; }

		public DistributedServiceType Type { get; set; }

		public string BaseAddress { get; set; }
		#endregion
	}
}

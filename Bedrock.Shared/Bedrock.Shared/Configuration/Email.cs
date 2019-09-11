using System.Collections.Generic;
using Bedrock.Shared.Enumeration;

namespace Bedrock.Shared.Configuration
{
	public class Email
	{
		#region Fields
		private Dictionary<string, string> _toAddresses;
		#endregion

		#region Properties
		public bool IsEnabled { get; set; }

		public string FromAddress { get; set; }

		public Dictionary<string, string> ToAddresses
		{
			get
			{
				_toAddresses = _toAddresses ?? new Dictionary<string, string>();
				return _toAddresses;
			}
			set { _toAddresses = value; }
		}

		public EmailSmtp Smtp { get; set; }
		#endregion
	}

	public class ToAddress
	{
		#region Properties
		public string Key { get; set; }

		public string Address { get; set; }
		#endregion
	}

	public class EmailSmtp
	{
		#region Properties
		public string Server { get; set; }

		public int Port { get; set; }

		public bool IsUseDefaultCredentials { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public EmailDeliveryMethod DeliveryMethod { get; set; }

		public string PickupDirectoryLocation { get; set; }
		#endregion
	}
}

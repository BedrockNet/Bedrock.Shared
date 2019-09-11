using System;
using System.Collections.Generic;

using Bedrock.Shared.Enumeration;

namespace Bedrock.Shared.Configuration
{
	public class Cache
	{
		#region Fields
		private List<CacheType> _configuredCaches;
		private Dictionary<string, TimeSpan> _cacheItems;
		#endregion

		#region Properties
		public bool IsCacheEnabled { get; set; }

		public CacheType CacheType { get; set; }

		public int CacheExpiry { get; set; }

		public int CacheExpiryHour { get; set; }

		public List<CacheType> ConfiguredCaches
		{
			get
			{
				_configuredCaches = _configuredCaches ?? new List<CacheType>();
				return _configuredCaches;
			}
			set { _configuredCaches = value; }
		}

		public Redis Redis { get; set; }

		public Dictionary<string, TimeSpan> CacheItems
		{
			get
			{
				_cacheItems = _cacheItems ?? new Dictionary<string, TimeSpan>();
				return _cacheItems;
			}
			set { _cacheItems = value; }
		}
		#endregion
	}

	public class Redis
	{
		#region Fields
		private List<string> _commands;
		private List<EndPoint> _endPoints;
		#endregion

		#region Public Properties
		public bool AbortOnConnectFail { get; set; }

		public bool AllowAdmin { get; set; }

		public string ChannelPrefix { get; set; }

		public string ClientName { get; set; }

		public List<string> Commands
		{
			get
			{
				_commands = _commands ?? new List<string>();
				return _commands;
			}
			set { _commands = value; }
		}

		public int ConfigCheckSeconds { get; set; }

		public string ConfigurationChannel { get; set; }

		public int ConnectRetry { get; set; }

		public int ConnectTimeout { get; set; }

		public int? DefaultDatabase { get; set; }

		public string DefaultVersion { get; set; }

		public List<EndPoint> EndPoints
		{
			get
			{
				_endPoints = _endPoints ?? new List<EndPoint>();
				return _endPoints;
			}
			set { _endPoints = value; }
		}

		public bool IsCommandsAvailable { get; set; }

		public int KeepAlive { get; set; }

		public string Password { get; set; }

		public Proxy Proxy { get; set; }

		public bool ResolveDns { get; set; }

		public int ResponseTimeout { get; set; }

		public string ServiceName { get; set; }

		public bool Ssl { get; set; }

		public string SslHost { get; set; }

		public int SyncTimeout { get; set; }

		public string TieBreaker { get; set; }
		#endregion
	}

	public class EndPoint
	{
		#region Public Properties
		public string Host { get; set; }

		public int Port { get; set; }
		#endregion
	}
}

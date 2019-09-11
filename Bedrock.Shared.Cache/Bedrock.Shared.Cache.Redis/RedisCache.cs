using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Bedrock.Shared.Cache.Extension;
using Bedrock.Shared.Cache.Redis.Interface;

using Bedrock.Shared.Configuration;
using Bedrock.Shared.Extension;

using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Newtonsoft;

using CacheEnumeration = Bedrock.Shared.Cache.Enumeration;
using SharedEnumeration = Bedrock.Shared.Enumeration;

namespace Bedrock.Shared.Cache.Redis
{
    public class RedisCache : CacheBase, ICacheRedis
    {
        #region Fields
        private static ICacheClient _client;
        private static object _lockSemaphore = new object();
        private static bool _isInitialized = false;
        #endregion

        #region Constructors
        public RedisCache(BedrockConfiguration bedrockConfiguration)
        {
            CacheType = SharedEnumeration.CacheType.Redis;
            BedrockConfiguration = bedrockConfiguration;
        }
        #endregion

        #region Properties
        protected ICacheClient Client
        {
            get
            {
                Initialize();
                return _client;
            }
        }

		protected BedrockConfiguration BedrockConfiguration { get; set; }
		#endregion

		#region ICache Methods
		public override T Get<T>(string cacheKey)
        {
            var returnValue = Client.Get<T>(cacheKey);

            if(returnValue == null)
                RemoveKey(cacheKey);

            return returnValue;
        }

        public override async Task<T> GetAsync<T>(string cacheKey)
        {
            var returnValue = await Client.GetAsync<T>(cacheKey);

            if (returnValue == null)
                RemoveKey(cacheKey);

            return returnValue;
        }

        public override void Add(string cacheKey, DateTime expiry, object dataToAdd)
        {
            if (dataToAdd != null)
            {
                Client.Add(cacheKey, dataToAdd, expiry);
                AddKey(cacheKey);
            }
        }

        public override async Task AddAsync(string cacheKey, DateTime expiry, object dataToAdd)
        {
            if (dataToAdd != null)
            {
                AddKey(cacheKey);
                await Client.AddAsync(cacheKey, dataToAdd, expiry);
            }
        }

        public override void Add(string cacheKey, TimeSpan slidingExpiryWindow, object dataToAdd)
        {
            if (dataToAdd != null)
            {
                Client.Add(cacheKey, dataToAdd, slidingExpiryWindow);
                AddKey(cacheKey);
            }
        }

        public override async Task AddAsync(string cacheKey, TimeSpan slidingExpiryWindow, object dataToAdd)
        {
            if (dataToAdd != null)
            {
                AddKey(cacheKey);
                await Client.AddAsync(cacheKey, dataToAdd, slidingExpiryWindow);
            }
        }

        public override void InvalidateCacheItem(string cacheKey)
        {
            Client.Remove(cacheKey);
            RemoveKey(cacheKey);
        }

        public override async Task InvalidateCacheItemAsync(string cacheKey)
        {
            RemoveKey(cacheKey);
            await Client.RemoveAsync(cacheKey);
        }
        #endregion

        #region IRedisCache Methods
        public bool Exists(string cacheKey)
        {
            return Client.Exists(cacheKey);
        }

        public async Task<bool> ExistsAsync(string cacheKey)
        {
            return await Client.ExistsAsync(cacheKey);
        }

        public void FlushDatabase()
        {
            var options = GetConfigurationOptions(true);

            using (var connection = GetConnection(options).Value)
            using (var client = new StackExchangeRedisCacheClient(connection, new NewtonsoftSerializer()))
                client.FlushDb();
        }

        public async Task FlushDatabaseAsync()
        {
            var options = GetConfigurationOptions(true);

            using (var connection = GetConnection(options).Value)
            using (var client = new StackExchangeRedisCacheClient(connection, new NewtonsoftSerializer()))
                await client.FlushDbAsync();
        }

        public IDictionary<string, T> GetAll<T>(params string[] cacheKeys) where T : class
        {
            return Client.GetAll<T>(cacheKeys);
        }

        public async Task<IDictionary<string, T>> GetAllAsync<T>(params string[] cacheKeys) where T : class
        {
            return await Client.GetAllAsync<T>(cacheKeys);
        }

        public Dictionary<string, string> GetInformation()
        {
            return Client.GetInfo();
        }

        public async Task<Dictionary<string, string>> GetInformationAsync()
        {
            return await Client.GetInfoAsync();
        }

        public void Replace<T>(string cacheKey, T value) where T : class
        {
            Client.Replace<T>(cacheKey, value);
        }

        public async Task ReplaceAsync<T>(string cacheKey, T value) where T : class
        {
            await Client.ReplaceAsync<T>(cacheKey, value);
        }

        public void Save(CacheEnumeration.SaveType saveType)
        {
            var options = GetConfigurationOptions(true);

            using (var connection = GetConnection(options).Value)
            using (var client = new StackExchangeRedisCacheClient(connection, new NewtonsoftSerializer()))
                client.Save((SaveType)saveType);
        }

        public void SaveAsync(CacheEnumeration.SaveType saveType)
        {
            var options = GetConfigurationOptions(true);

            using (var connection = GetConnection(options).Value)
            using (var client = new StackExchangeRedisCacheClient(connection, new NewtonsoftSerializer()))
                client.SaveAsync((SaveType)saveType);
        }

        public IEnumerable<string> SearchKeys(string pattern)
        {
            return Client.SearchKeys(pattern);
        }

        public IEnumerable<string> SearchKeys(string pattern, CacheEnumeration.WildCard wildCard)
        {
            return Client.SearchKeys(string.Format("{1}{0}{1}", pattern, CacheEnumeration.WildCard.QuestionMark.ToSymbolString()));
        }

        public async Task<IEnumerable<string>> SearchKeysAsync(string pattern)
        {
            return await Client.SearchKeysAsync(pattern);
        }

        public async Task<IEnumerable<string>> SearchKeysAsync(string pattern, CacheEnumeration.WildCard wildCard)
        {
            return await Client.SearchKeysAsync(string.Format("{1}{0}{1}", pattern, CacheEnumeration.WildCard.QuestionMark.ToSymbolString()));
        }

        public void Reset()
        {
            if (_client != null)
                _client.Dispose();

            _client = null;
            _isInitialized = false;

            Initialize();
        }
        #endregion

        #region Private Methods
        private void Initialize()
        {
			if (!_isInitialized)
            {
                lock (_lockSemaphore)
                {
                    if (!_isInitialized)
                    {
                        if (_client != null)
                        {
                            _isInitialized = true;
                            return;
                        }

                        var connection = GetConnection().Value;
                        _client = new StackExchangeRedisCacheClient(connection, new NewtonsoftSerializer());

                        _isInitialized = true;
                    }
                }
            }
        }

        private Lazy<ConnectionMultiplexer> GetConnection(ConfigurationOptions configOptions = null) => new Lazy<ConnectionMultiplexer>(() =>
        {
            var options = configOptions != null ? configOptions : GetConfigurationOptions();
            var connection = ConnectionMultiplexer.Connect(options);

            return connection;
        });

        private ConfigurationOptions GetConfigurationOptions(bool isForceAllowAdmin = false)
        {
            var endPointFirst = BedrockConfiguration.Cache.Redis.EndPoints.First();

            var returnValue = new ConfigurationOptions
            {
                AbortOnConnectFail = BedrockConfiguration.Cache.Redis.AbortOnConnectFail,
                AllowAdmin = isForceAllowAdmin ? true : BedrockConfiguration.Cache.Redis.AllowAdmin,
                ChannelPrefix = BedrockConfiguration.Cache.Redis.ChannelPrefix,
                ClientName = BedrockConfiguration.Cache.Redis.ClientName,
                CommandMap = CommandMap.Create(new HashSet<string>(BedrockConfiguration.Cache.Redis.Commands), available: BedrockConfiguration.Cache.Redis.IsCommandsAvailable),
                ConfigCheckSeconds = BedrockConfiguration.Cache.Redis.ConfigCheckSeconds,
                ConfigurationChannel = BedrockConfiguration.Cache.Redis.ConfigurationChannel,
                ConnectRetry = BedrockConfiguration.Cache.Redis.ConnectRetry,
                ConnectTimeout = BedrockConfiguration.Cache.Redis.ConnectTimeout,
                DefaultDatabase = BedrockConfiguration.Cache.Redis.DefaultDatabase,
                DefaultVersion = new Version(BedrockConfiguration.Cache.Redis.DefaultVersion),
                KeepAlive = BedrockConfiguration.Cache.Redis.KeepAlive,
                Password = BedrockConfiguration.Cache.Redis.Password,
                Proxy = (Proxy)BedrockConfiguration.Cache.Redis.Proxy,
                ResolveDns = BedrockConfiguration.Cache.Redis.ResolveDns,
                ResponseTimeout = BedrockConfiguration.Cache.Redis.ResponseTimeout,
                ServiceName = BedrockConfiguration.Cache.Redis.ServiceName,
                Ssl = BedrockConfiguration.Cache.Redis.Ssl,
                SslHost = BedrockConfiguration.Cache.Redis.SslHost,
                SyncTimeout = BedrockConfiguration.Cache.Redis.SyncTimeout,
                TieBreaker = BedrockConfiguration.Cache.Redis.TieBreaker
            };

            SetEndPoints(returnValue);

            return returnValue;
        }

        private void SetEndPoints(ConfigurationOptions options)
        {
            var endPoints = new EndPointCollection();

            BedrockConfiguration.Cache.Redis.EndPoints.Each(ep => endPoints.Add(ep.Host, ep.Port));

            options
                .GetType()
                .GetField("endpoints", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(options, endPoints);
        }
        #endregion
    }
}
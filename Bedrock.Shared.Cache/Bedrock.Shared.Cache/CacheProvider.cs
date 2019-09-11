using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Bedrock.Shared.Cache.Interface;
using Bedrock.Shared.Configuration;
using Bedrock.Shared.Enumeration;
using Bedrock.Shared.Extension;

namespace Bedrock.Shared.Cache
{
    public class CacheProvider : ICacheProvider
    {
        #region Constructors
        public CacheProvider(ICache[] cachers, BedrockConfiguration bedrockConfiguration) : this(cachers, bedrockConfiguration.Cache.CacheType, bedrockConfiguration) { }

        public CacheProvider(ICache[] cachers, CacheType cacheType, BedrockConfiguration bedrockConfiguration)
        {
            Initialize(cachers, cacheType, bedrockConfiguration);
        }
        #endregion

        #region ICacheProvider Properties
        public ICache InnerCache { get; set; }
        #endregion

        #region Protected Properties
        protected bool IsCacheEnabled
        {
            get { return BedrockConfiguration.Cache.IsCacheEnabled; }
        }

        protected IEnumerable<ICache> Cachers { get; set; }

		protected BedrockConfiguration BedrockConfiguration { get; set; }
		#endregion

		#region ICacheProvider Methods
		public T Get<T>(string cacheKey, DateTime expiryDate, Func<T> getData) where T : class
        {
            T data = GetData(cacheKey, getData);
            Add(cacheKey, expiryDate, data);

            return data;
        }

        public async Task<T> GetAsync<T>(string cacheKey, DateTime expiryDate, Func<T> getData) where T : class
        {
            var data = await GetDataAsync(cacheKey, getData);
            await AddAsync(cacheKey, expiryDate, data);

            return data;
        }

        public T Get<T>(string cacheKey, TimeSpan slidingExpiryWindow, Func<T> getData) where T : class
        {
            T data = GetData(cacheKey, getData);
            Add(cacheKey, slidingExpiryWindow, data);

            return data;
        }

        public async Task<T> GetAsync<T>(string cacheKey, TimeSpan slidingExpiryWindow, Func<T> getData) where T : class
        {
            var data = await GetDataAsync(cacheKey, getData);
            await AddAsync(cacheKey, slidingExpiryWindow, data);

            return data;
        }

        public T Get<T>(DateTime absoluteExpiryDate, Func<T> getData) where T : class
        {
            return Get<T>(GetCacheKeyFromFuncDelegate(getData), absoluteExpiryDate, getData);
        }

        public async Task<T> GetAsync<T>(DateTime absoluteExpiryDate, Func<T> getData) where T : class
        {
            return await GetAsync<T>(GetCacheKeyFromFuncDelegate(getData), absoluteExpiryDate, getData);
        }

        public T Get<T>(TimeSpan slidingExpiryWindow, Func<T> getData) where T : class
        {
            return Get<T>(GetCacheKeyFromFuncDelegate(getData), slidingExpiryWindow, getData);
        }

        public async Task<T> GetAsync<T>(TimeSpan slidingExpiryWindow, Func<T> getData) where T : class
        {
            return await GetAsync<T>(GetCacheKeyFromFuncDelegate(getData), slidingExpiryWindow, getData);
        }

        public T Get<E, M, T>(string cacheKey, DateTime expiryDate, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class
        {
            T data = GetData(cacheKey, getData, transformData);
            Add(cacheKey, expiryDate, data);

            return data;
        }

        public async Task<T> GetAsync<E, M, T>(string cacheKey, DateTime expiryDate, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class
        {
            var data = await GetDataAsync(cacheKey, getData, transformData);
            await AddAsync(cacheKey, expiryDate, data);

            return data;
        }

        public T Get<E, M, T>(string cacheKey, TimeSpan slidingExpiryWindow, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class
        {
            T data = GetData(cacheKey, getData, transformData);
            Add(cacheKey, slidingExpiryWindow, data);

            return data;
        }

        public async Task<T> GetAsync<E, M, T>(string cacheKey, TimeSpan slidingExpiryWindow, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class
        {
            T data = await GetDataAsync(cacheKey, getData, transformData);
            await AddAsync(cacheKey, slidingExpiryWindow, data);

            return data;
        }

        public T Get<E, M, T>(DateTime absoluteExpiryDate, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class
        {
            return Get<E, M, T>(GetCacheKeyFromFuncDelegate(getData), absoluteExpiryDate, getData, transformData);
        }

        public async Task<T> GetAsync<E, M, T>(DateTime absoluteExpiryDate, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class
        {
            return await GetAsync<E, M, T>(GetCacheKeyFromFuncDelegate(getData), absoluteExpiryDate, getData, transformData);
        }

        public T Get<E, M, T>(TimeSpan slidingExpiryWindow, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class
        {
            return Get<E, M, T>(GetCacheKeyFromFuncDelegate(getData), slidingExpiryWindow, getData, transformData);
        }

        public async Task<T> GetAsync<E, M, T>(TimeSpan slidingExpiryWindow, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class
        {
            return await GetAsync<E, M, T>(GetCacheKeyFromFuncDelegate(getData), slidingExpiryWindow, getData, transformData);
        }

        public void Add(string cacheKey, DateTime absoluteExpiryDate, object dataToAdd)
        {
            if (dataToAdd == null || !IsCacheEnabled)
                return;

            InnerCache.Add(cacheKey, absoluteExpiryDate, dataToAdd);
        }

        public async Task AddAsync(string cacheKey, DateTime absoluteExpiryDate, object dataToAdd)
        {
            if (dataToAdd == null || !IsCacheEnabled)
                await Task.Delay(0);

            await InnerCache.AddAsync(cacheKey, absoluteExpiryDate, dataToAdd);
        }

        public void Add(string cacheKey, TimeSpan slidingExpiryWindow, object dataToAdd)
        {
            if (dataToAdd == null || !IsCacheEnabled)
                return;

            InnerCache.Add(cacheKey, slidingExpiryWindow, dataToAdd);
        }

        public async Task AddAsync(string cacheKey, TimeSpan slidingExpiryWindow, object dataToAdd)
        {
            if (dataToAdd == null || !IsCacheEnabled)
                await Task.Delay(0);

            await InnerCache.AddAsync(cacheKey, slidingExpiryWindow, dataToAdd);
        }

        public void InvalidateCacheItem(string cacheKey)
        {
            if (!IsCacheEnabled)
                return;

            InnerCache.InvalidateCacheItem(cacheKey);
        }

        public async Task InvalidateCacheItemAsync(string cacheKey)
        {
            if (!IsCacheEnabled)
                await Task.Delay(0);

            await InnerCache.InvalidateCacheItemAsync(cacheKey);
        }

        public void InvalidateCache()
        {
            if (!IsCacheEnabled)
                return;

            InnerCache.KeyDictionary.Each(kv => InvalidateCacheItem(kv.Key));
            InnerCache.KeyDictionary.Clear();
        }

        public async Task InvalidateCacheAsync()
        {
            if (!IsCacheEnabled)
                await Task.Delay(0);

            var tasks = new List<Task>();

            InnerCache.KeyDictionary.Each(kv => tasks.Add(InvalidateCacheItemAsync(kv.Key)));
            InnerCache.KeyDictionary.Clear();

            await Task.WhenAll(tasks);
        }

        public ICache GetCache(CacheType cacheType)
        {
            return Cachers.First(s => s.CacheType == cacheType);
        }

        public void SetCacheType(CacheType cacheType)
        {
            InnerCache = Cachers.First(s => s.CacheType == cacheType);
        }
        #endregion

        #region Private Methods
        private T GetData<T>(string cacheKey, Func<T> getData) where T : class
        {
            T data = IsCacheEnabled ? InnerCache.Get<T>(cacheKey) : null;

            if (data == null)
                data = getData();

            return data;
        }

        private async Task<T> GetDataAsync<T>(string cacheKey, Func<T> getData) where T : class
        {
            var data = IsCacheEnabled ? InnerCache.GetAsync<T>(cacheKey) : Task.FromResult(default(T));

            return await data.ContinueWith<T>(t =>
            {
                var returnValue = t.Result;

                if (t.Result == null)
                    returnValue = getData();

                return returnValue;
            });
        }

        private string GetCacheKeyFromFuncDelegate<T>(Func<T> getData) where T : class
        {
            return string.Concat(getData.Method.DeclaringType.FullName, "-", getData.Method.Name);
        }

        private T GetData<E, M, T>(string cacheKey, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class
        {
            T data = IsCacheEnabled ? InnerCache.Get<T>(cacheKey) : null;

            if (data == null)
                data = getData(transformData);

            return data;
        }

        private async Task<T> GetDataAsync<E, M, T>(string cacheKey, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class
        {
            var data = IsCacheEnabled ? InnerCache.GetAsync<T>(cacheKey) : Task.FromResult(default(T));

            return await data.ContinueWith<T>(t =>
            {
                var returnValue = t.Result;

                if (t.Result == null)
                    returnValue = getData(transformData);

                return returnValue;
            });
        }

        private string GetCacheKeyFromFuncDelegate<E, M, T>(Func<Func<E, M>, T> getData) where T : class
        {
            return string.Concat(getData.Method.DeclaringType.FullName, "-", getData.Method.Name);
        }

        private void Initialize(ICache[] cachers, CacheType cacheType, BedrockConfiguration bedrockConfiguration)
        {
			BedrockConfiguration = bedrockConfiguration;
            Cachers = cachers;
            SetCacheType(cacheType);
        }
        #endregion
    }
}

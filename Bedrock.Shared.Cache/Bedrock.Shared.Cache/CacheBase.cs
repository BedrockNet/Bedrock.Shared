using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using Bedrock.Shared.Cache.Interface;
using Bedrock.Shared.Enumeration;

namespace Bedrock.Shared.Cache
{
    public abstract class CacheBase : ICache
    {
        #region Constructors
        public CacheBase()
        {
            KeyDictionary = new ConcurrentDictionary<string, bool>();
        }
        #endregion

        #region ICache Properties
        public virtual CacheType CacheType { get; set; }

        public ConcurrentDictionary<string, bool> KeyDictionary { get; set; }
        #endregion

        #region ICache Methods
        public abstract T Get<T>(string cacheKey) where T : class;

        public abstract Task<T> GetAsync<T>(string cacheKey) where T : class;

        public abstract void Add(string cacheKey, DateTime absoluteExpiry, object dataToAdd);

        public abstract Task AddAsync(string cacheKey, DateTime absoluteExpiry, object dataToAdd);

        public abstract void Add(string cacheKey, TimeSpan slidingExpiryWindow, object dataToAdd);

        public abstract Task AddAsync(string cacheKey, TimeSpan slidingExpiryWindow, object dataToAdd);

        public abstract void InvalidateCacheItem(string cacheKey);

        public abstract Task InvalidateCacheItemAsync(string cacheKey);
        #endregion

        #region Protected Methods
        protected void AddKey(string cacheKey)
        {
            if (!KeyDictionary.ContainsKey(cacheKey))
                KeyDictionary.AddOrUpdate(cacheKey, true, (k, v) => true);
        }

        protected void RemoveKey(string cacheKey)
        {
            var value = default(bool);
            KeyDictionary.TryRemove(cacheKey, out value);
        }
        #endregion
    }
}
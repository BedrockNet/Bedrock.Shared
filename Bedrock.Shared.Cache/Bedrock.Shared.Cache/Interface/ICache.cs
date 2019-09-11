using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using Bedrock.Shared.Enumeration;

namespace Bedrock.Shared.Cache.Interface
{
    public interface ICache
    {
        #region Properties
        CacheType CacheType { get; }

        ConcurrentDictionary<string, bool> KeyDictionary { get; set; }
        #endregion

        #region Methods
        T Get<T>(string cacheKey) where T : class;

        Task<T> GetAsync<T>(string cacheKey) where T : class;

        void Add(string cacheKey, DateTime absoluteExpiry, object dataToAdd);

        Task AddAsync(string cacheKey, DateTime absoluteExpiry, object dataToAdd);

        void Add(string cacheKey, TimeSpan slidingExpiryWindow, object dataToAdd);

        Task AddAsync(string cacheKey, TimeSpan slidingExpiryWindow, object dataToAdd);

        void InvalidateCacheItem(string cacheKey);

        Task InvalidateCacheItemAsync(string cacheKey);
        #endregion
    }
}

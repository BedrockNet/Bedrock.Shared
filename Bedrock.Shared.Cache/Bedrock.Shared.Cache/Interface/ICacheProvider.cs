using System;
using System.Threading.Tasks;

using Bedrock.Shared.Enumeration;

namespace Bedrock.Shared.Cache.Interface
{
    public interface ICacheProvider
    {
        #region Properties
        ICache InnerCache { get; }
        #endregion

        #region Methods
        T Get<T>(string cacheKey, DateTime absoluteExpiryDate, Func<T> getData) where T : class;

        Task<T> GetAsync<T>(string cacheKey, DateTime absoluteExpiryDate, Func<T> getData) where T : class;

        T Get<T>(string cacheKey, TimeSpan slidingExpiryWindow, Func<T> getData) where T : class;

        Task<T> GetAsync<T>(string cacheKey, TimeSpan slidingExpiryWindow, Func<T> getData) where T : class;

        T Get<T>(DateTime absoluteExpiryDate, Func<T> getData) where T : class;

        Task<T> GetAsync<T>(DateTime absoluteExpiryDate, Func<T> getData) where T : class;

        T Get<T>(TimeSpan slidingExpiryWindow, Func<T> getData) where T : class;

        Task<T> GetAsync<T>(TimeSpan slidingExpiryWindow, Func<T> getData) where T : class;

        T Get<E, M, T>(string cacheKey, DateTime absoluteExpiryDate, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class;

        Task<T> GetAsync<E, M, T>(string cacheKey, DateTime absoluteExpiryDate, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class;

        T Get<E, M, T>(string cacheKey, TimeSpan slidingExpiryWindow, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class;

        Task<T> GetAsync<E, M, T>(string cacheKey, TimeSpan slidingExpiryWindow, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class;

        T Get<E, M, T>(DateTime absoluteExpiryDate, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class;

        Task<T> GetAsync<E, M, T>(DateTime absoluteExpiryDate, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class;

        T Get<E, M, T>(TimeSpan slidingExpiryWindow, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class;

        Task<T> GetAsync<E, M, T>(TimeSpan slidingExpiryWindow, Func<Func<E, M>, T> getData, Func<E, M> transformData) where T : class;

        void Add(string cacheKey, DateTime absoluteExpiryDate, object dataToAdd);

        Task AddAsync(string cacheKey, DateTime absoluteExpiryDate, object dataToAdd);

        void Add(string cacheKey, TimeSpan slidingExpiryWindow, object dataToAdd);

        Task AddAsync(string cacheKey, TimeSpan slidingExpiryWindow, object dataToAdd);

        void InvalidateCacheItem(string cacheKey);

        Task InvalidateCacheItemAsync(string cacheKey);

        void InvalidateCache();

        Task InvalidateCacheAsync();

        ICache GetCache(CacheType cacheType);

        void SetCacheType(CacheType cacheType);
        #endregion
    }
}

using System;
using System.Threading.Tasks;

using Bedrock.Shared.Cache.Memory.Interface;
using Bedrock.Shared.Enumeration;

using Microsoft.Extensions.Caching.Memory;
using RuntimeCaching = Microsoft.Extensions.Caching.Memory;

namespace Bedrock.Shared.Cache.Memory
{
	public class MemoryCache : CacheBase, ICacheMemory
	{
		#region Fields
		private RuntimeCaching.MemoryCache _cache;
		#endregion

		#region Constructors
		public MemoryCache()
		{
			CacheType = CacheType.Memory;
		}
		#endregion

		#region Properties
		protected IMemoryCache Cache
		{
			get
			{
				if (_cache == null)
					_cache = new RuntimeCaching.MemoryCache(new MemoryCacheOptions());

				return _cache;
			}
		}
		#endregion

		#region ICache Methods
		public override T Get<T>(string cacheKey)
		{
			var returnValue = Cache.Get(cacheKey) as T;

			if (returnValue == null)
				RemoveKey(cacheKey);

			return returnValue;
		}

		public override async Task<T> GetAsync<T>(string cacheKey)
		{
			var result = Get<T>(cacheKey);
			var returnValue = await Task.FromResult(result);

			if (returnValue == null)
				RemoveKey(cacheKey);

			return returnValue;
		}

		public override void Add(string cacheKey, DateTime expiry, object dataToAdd)
		{
			if (dataToAdd != null)
			{
				Cache.Set(cacheKey, dataToAdd, new MemoryCacheEntryOptions().SetAbsoluteExpiration(expiry));
				AddKey(cacheKey);
			}
		}

		public override async Task AddAsync(string cacheKey, DateTime expiry, object dataToAdd)
		{
			Add(cacheKey, expiry, dataToAdd);
			AddKey(cacheKey);

			await Task.Delay(0);
		}

		public override void Add(string cacheKey, TimeSpan slidingExpiryWindow, object dataToAdd)
		{
			if (dataToAdd != null)
			{
				Cache.Set(cacheKey, dataToAdd, new MemoryCacheEntryOptions().SetSlidingExpiration(slidingExpiryWindow));
				AddKey(cacheKey);
			}
		}

		public override async Task AddAsync(string cacheKey, TimeSpan slidingExpiryWindow, object dataToAdd)
		{
			Add(cacheKey, slidingExpiryWindow, dataToAdd);
			AddKey(cacheKey);

			await Task.Delay(0);
		}

		public override void InvalidateCacheItem(string cacheKey)
		{
			Cache.Remove(cacheKey);
			RemoveKey(cacheKey);
		}

		public override async Task InvalidateCacheItemAsync(string cacheKey)
		{
			InvalidateCacheItem(cacheKey);
			await Task.Delay(0);
		}
		#endregion
	}
}

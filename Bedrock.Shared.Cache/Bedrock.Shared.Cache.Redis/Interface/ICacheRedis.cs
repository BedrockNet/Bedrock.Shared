using System.Collections.Generic;
using System.Threading.Tasks;

using Bedrock.Shared.Cache.Enumeration;
using Bedrock.Shared.Cache.Interface;

namespace Bedrock.Shared.Cache.Redis.Interface
{
    public interface ICacheRedis : ICache
    {
        #region Methods
        bool Exists(string cacheKey);

        Task<bool> ExistsAsync(string cacheKey);

        void FlushDatabase();

        Task FlushDatabaseAsync();

        IDictionary<string, T> GetAll<T>(params string[] cacheKeys) where T : class;

        Task<IDictionary<string, T>> GetAllAsync<T>(params string[] cacheKeys) where T : class;

        Dictionary<string, string> GetInformation();

        Task<Dictionary<string, string>> GetInformationAsync();

        void Replace<T>(string cacheKey, T value) where T : class;

        Task ReplaceAsync<T>(string cacheKey, T value) where T : class;

        void Save(SaveType saveType);

        void SaveAsync(SaveType saveType);

        IEnumerable<string> SearchKeys(string pattern);

        IEnumerable<string> SearchKeys(string pattern, WildCard wildCard);

        Task<IEnumerable<string>> SearchKeysAsync(string pattern);

        Task<IEnumerable<string>> SearchKeysAsync(string pattern, WildCard wildCard);

        void Reset();
        #endregion
    }
}

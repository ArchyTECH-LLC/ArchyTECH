using System.Runtime.Caching;

namespace ArchyTECH.Core.Caching
{
    public interface ICache
    {
        T? Get<T>(string key);
        T AddOrGet<T>(string key, Func<T> refreshCallback, int timeoutMinutes, bool slidingTimeout = false, bool autoRefresh = false);
        Task<T> AddOrGet<T>(string key, Func<Task<T>> refreshCallback, int timeoutMinutes, bool slidingTimeout = false,
            bool autoRefresh = false);
        T Set<T>(string key, Func<T> refreshCallback, int timeoutMinutes, bool slidingTimeout = false, bool autoRefresh = false);
        Task<T> Set<T>(string key, Func<Task<T>> refreshCallback, int timeoutMinutes, bool slidingTimeout = false,
            bool autoRefresh = false);
        void Remove(string key);
        T? Remove<T>(string key);
        void ClearAll();
    }

    public class Cache : ICache
    {
        private MemoryCache _memoryCache;

        public Cache(string name)
        {
            _memoryCache = new MemoryCache(name);
        }

        public T AddOrGet<T>(string key, Func<T> refreshCallback, int timeoutMinutes, bool slidingTimeout = false,
            bool autoRefresh = false)
        {
            var cacheItem = _memoryCache.GetCacheItem(key);

            if (cacheItem != null)
            {
                return (T)cacheItem.Value;
            }
            return Set(key, refreshCallback, timeoutMinutes, slidingTimeout, autoRefresh);
        }

        public async Task<T> AddOrGet<T>(string key, Func<Task<T>> refreshCallback, int timeoutMinutes, bool slidingTimeout = false,
            bool autoRefresh = false)
        {
            var cacheItem = _memoryCache.GetCacheItem(key);

            if (cacheItem != null)
            {
                return (T)cacheItem.Value;
            }
            return await Set(key, refreshCallback, timeoutMinutes, slidingTimeout, autoRefresh);
        }

        public  T? Get<T>(string key)
        {
            return (T?)_memoryCache.Get(key);
        }
        
        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public T? Remove<T>(string key)
        {
            return (T?)_memoryCache.Remove(key);
        }

        public T Set<T>(string key, Func<T> refreshCallback, int timeoutMinutes, bool slidingTimeout = false, bool autoRefresh = false)
        {
            if (refreshCallback == null) throw new ArgumentNullException(nameof(refreshCallback));

            var callbackPolicy = CreateCallbackPolicy(key, refreshCallback, timeoutMinutes, slidingTimeout, autoRefresh);
            var result = refreshCallback();

            if (result == null) return result;

            _memoryCache.Set(key, result, callbackPolicy);
            return result;
        }

        public async Task<T> Set<T>(string key, Func<Task<T>> refreshCallback, int timeoutMinutes, bool slidingTimeout = false, bool autoRefresh = false)
        {
            if (refreshCallback == null) throw new ArgumentNullException(nameof(refreshCallback));

            var callbackPolicy = CreateCallbackPolicy(key, refreshCallback, timeoutMinutes, slidingTimeout, autoRefresh);
            var result = await refreshCallback();

            if (result == null) return result;

            _memoryCache.Set(key, result, callbackPolicy);
            return result;
        }
        public void ClearAll()
        {
            _memoryCache.Dispose();
            _memoryCache = new MemoryCache("applicationCache");
        }

        private CacheItemPolicy CreateCallbackPolicy<T>(string key, Func<Task<T>> refreshCallback, int timeoutMinutes, bool slidingTimeout, bool autoRefresh)
        {
            var callbackPolicy = new CacheItemPolicy();
            if (autoRefresh)
            {
                callbackPolicy.UpdateCallback = async arguments =>
                {
                    var value = await refreshCallback();
                    arguments.UpdatedCacheItem = new CacheItem(key, value);
                    arguments.UpdatedCacheItemPolicy = CreateCallbackPolicy(key, refreshCallback, timeoutMinutes,
                        slidingTimeout, true);
                };
            }

            if (slidingTimeout)
            {
                callbackPolicy.SlidingExpiration = TimeSpan.FromMinutes(timeoutMinutes);
            }
            else
            {
                callbackPolicy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(timeoutMinutes);
            }

            return callbackPolicy;
        }

        private CacheItemPolicy CreateCallbackPolicy<T>(string key, Func<T> refreshCallback, int timeoutMinutes, bool slidingTimeout, bool autoRefresh)
        {
            var callbackPolicy = new CacheItemPolicy();
            if (autoRefresh)
            {
                callbackPolicy.UpdateCallback = arguments =>
                {
                    arguments.UpdatedCacheItem = new CacheItem(key, refreshCallback());
                    arguments.UpdatedCacheItemPolicy = CreateCallbackPolicy(key, refreshCallback, timeoutMinutes,
                        slidingTimeout, true);
                };
            }

            if (slidingTimeout)
            {
                callbackPolicy.SlidingExpiration = TimeSpan.FromMinutes(timeoutMinutes);
            }
            else
            {
                callbackPolicy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(timeoutMinutes);
            }

            return callbackPolicy;
        }
    }
}
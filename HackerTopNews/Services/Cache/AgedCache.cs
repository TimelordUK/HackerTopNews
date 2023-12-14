using HackerTopNews.Model;
using HackerTopNews.Services.Clock;
using System.Collections.Concurrent;

namespace HackerTopNews.Services.Cache
{
    /*
     * a cache with time expiry associated with each item placed in the cache such that
     * once culled a new latest version is requested to replace the one ejected.  This allows
     * for example a web service to be encapsulated such that many requests for same items
     * over a window of time will yield cached versions preventing massive load placed on 
     * a data source.
     */
    internal abstract class AgedCache<K, V> where V : class
    {
        readonly ConcurrentDictionary<K, CachedItem> _cachedItems = new();
        public int Count => _cachedItems.Count;
        private IServiceClock _clock;
        protected DateTime _lastCull;
        private object _lock = new object();
        private TimeSpan _itemLifeTime;
        public TimeSpan ItemLifeTime => _itemLifeTime;  

        protected AgedCache(IServiceClock clock, TimeSpan itemLifeTime)
        {
            _clock = clock;
            _lastCull = _clock.CurrentTime;
            _itemLifeTime = itemLifeTime;
        }

        protected AgedCache(IServiceClock clock, IConfiguration configuration, string key)
        {
            _clock = clock;
            _lastCull = _clock.CurrentTime;
            _itemLifeTime = GetExpire(configuration, key);
        }

        private static TimeSpan GetExpire(IConfiguration configuration, string key)
        {
            const int def = 60;
            var v = configuration[key] ?? $"{def}";
            var s = int.TryParse(v, out var expireSecs) ? expireSecs : def;
            return TimeSpan.FromSeconds(s);
        }

        public abstract Task<V> Get(K id);
        public struct CachedItem
        {
            public DateTime StoredAt { get; }
            public V Item { get; }
            public CachedItem(V item, DateTime storedAt)
            {
                Item = item;
                StoredAt = storedAt;
            }
            public bool IsExpired(IServiceClock clock, TimeSpan timeSpan)
            {
                return clock.CurrentTime - StoredAt > timeSpan;
            }
            public override string ToString()
            {
                return $"CachedItem StoredAt = {StoredAt}, Item = [{Item}]";
            }
        }

        private void Cull()
        {
            lock (_lock)
            {
                if (_clock.CurrentTime - _lastCull < TimeSpan.FromSeconds(_itemLifeTime.Seconds / 4)) return;
                var expired = _cachedItems.Where(kv => kv.Value.IsExpired(_clock, _itemLifeTime)).ToList();
                foreach (var item in expired)
                {
                    _cachedItems.TryRemove(item.Key, out _);
                }
                _lastCull = _clock.CurrentTime;
                if (expired.Count > 0)
                {
                    OnCulled(expired.Count);
                }
            }
        }

        protected abstract void OnCulled(int items);

        public async Task<V> GetOrAdd(K key, Func<K, Task<V>> maker)
        {
            Cull();
            if (!_cachedItems.TryGetValue(key, out var cachedItem))
            {
                var v = await maker(key);
                cachedItem = new CachedItem(v, _clock.CurrentTime);
                _cachedItems[key] = cachedItem;
            }
            return cachedItem.Item;
        }
    }
}

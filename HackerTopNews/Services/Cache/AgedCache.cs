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
        private readonly ConcurrentDictionary<K, CachedItem> _cachedItems = new();
        public int Count => _cachedItems.Count;
        private readonly IServiceClock _clock;
        protected DateTime _lastCull;
        private readonly object _lock = new object();
        private readonly TimeSpan _itemLifeTime;
        private readonly int _cullFrequency;
        public TimeSpan ItemLifeTime => _itemLifeTime;  

        protected AgedCache(IServiceClock clock, IConfiguration configuration, string key)
        {
            _clock = clock;
            _lastCull = _clock.CurrentTime;
            _itemLifeTime = TimeSpan.FromSeconds(configuration.GetAsInt32(key, 60));
            _cullFrequency = configuration.GetAsInt32("NewsCache:CullFrequency", 5);
        }


        public abstract Task<V> Get(K id);
        private readonly struct CachedItem
        {
            private DateTime StoredAt { get; }
            public Task<V> Item { get; }
            public CachedItem(Task<V> item, DateTime storedAt)
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
                var secs = _itemLifeTime.Seconds / _cullFrequency;
                if (_clock.CurrentTime - _lastCull < TimeSpan.FromSeconds(secs)) return;
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

        public Task<V> GetOrAdd(K key, Func<K, Task<V>> maker)
        {
            Cull();
            var cached = _cachedItems.GetOrAdd(key, _ =>
            {
                var v = maker(key);
                return new CachedItem(v, _clock.CurrentTime);

            });
            
            return cached.Item;
        }
    }
}

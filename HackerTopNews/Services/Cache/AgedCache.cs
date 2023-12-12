using HackerTopNews.Model;
using HackerTopNews.Services.Clock;
using System.Collections.Concurrent;

namespace HackerTopNews.Services.Cache
{
    public abstract class AgedCache<K, V> where V : class
    {
        readonly ConcurrentDictionary<K, CachedItem> _cachedItems = new();
        public int Count => _cachedItems.Count;
        private IServiceClock _clock;
        private DateTime _lastCull;
        private object _lock = new object();
        private TimeSpan _itemLifeTime;

        protected AgedCache(IServiceClock clock, TimeSpan itemLifeTime)
        {
            _clock = clock;
            _lastCull = _clock.CurrentTime;
            _itemLifeTime = itemLifeTime;
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
            }
        }

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

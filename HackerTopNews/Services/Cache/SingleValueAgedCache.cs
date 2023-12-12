
using HackerTopNews.Services.Clock;

namespace HackerTopNews.Services.Cache
{
    public abstract class SingleValueAgedCache<V> : AgedCache<int, V> where V : class
    {
        protected SingleValueAgedCache(IServiceClock clock, TimeSpan itemLifeTime) : base(clock, itemLifeTime) { }
        public override Task<V> Get(int id)
        {
            return Get();
        }
        public abstract Task<V> Get();
       
    }
}

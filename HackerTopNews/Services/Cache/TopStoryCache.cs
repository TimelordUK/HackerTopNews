using HackerTopNews.Model;
using HackerTopNews.Services.Clock;

namespace HackerTopNews.Services.Cache
{
    internal class TopStoryCache : AgedCache<int, IReadOnlyList<int>>, ITopStoryCache
    {
        private IHackerNewsService _hackerNewsWebService;
        private ILogger<NewsStoryCache> _logger;

        public TopStoryCache(ILogger<NewsStoryCache> logger, IServiceClock clock, IHackerNewsService hackerNewsWebService, IConfiguration configuration) : base(clock, configuration, "NewsCache:TopNewsExpireSeconds")
        {
            _logger = logger;
            _hackerNewsWebService = hackerNewsWebService;
            _logger.LogInformation($"TopStoryCache expiry lifetime = {ItemLifeTime}");
        }


        public override Task<IReadOnlyList<int>> Get(int id)
        {
            _logger.LogDebug($"TopStoryCache id = {id}");
            return GetOrAdd(id, _ =>
            {
                return _hackerNewsWebService.GetTopStories();
            });
        }

        public Task<IReadOnlyList<int>> Get()
        {
            return Get(0);
        }

        protected override void OnCulled(int items)
        {
            _logger.LogInformation($"TopStoryCache.OnCulled itemLifeTime = {ItemLifeTime}, lastcull = {_lastCull}, culled cache items = {items}, Count = {Count}");
        }
    }
}

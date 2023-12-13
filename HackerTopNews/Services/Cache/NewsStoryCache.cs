using HackerTopNews.Model;
using HackerTopNews.Services.Clock;

namespace HackerTopNews.Services.Cache
{
    /*
     * A time expired cache of items where they can live in cache for a 
     * defined period and the cached result will be returned, else the injected
     * service is used to resolve the story.  When under load this will save
     * sending huge numbers of requests for the same data.
     */
    internal class NewsStoryCache : AgedCache<int, HackerNewStory>, INewsStoryCache
    {
        private IHackerNewsService _hackerNewsWebService;
        private ILogger<NewsStoryCache> _logger;

        public NewsStoryCache(ILogger<NewsStoryCache> logger, IServiceClock clock, IHackerNewsService hackerNewsWebService) : base(clock, TimeSpan.FromSeconds(60))
        {
            _logger = logger;
            _hackerNewsWebService = hackerNewsWebService;
        }

        public override Task<HackerNewStory> Get(int id)
        {
            _logger.LogDebug($"NewStoryCache id = {id}");
            return GetOrAdd(id, newId =>
            {
                return _hackerNewsWebService.GetStory(id);
            });
        }

        protected override void OnCulled(int items)
        {
            _logger.LogInformation($"OnCulled lastcull = {_lastCull} culled cache items = {items} Count now = {Count}");
        }
    }
}


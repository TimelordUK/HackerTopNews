using HackerTopNews.Model;

namespace HackerTopNews.Services
{
    /*
     * A time expired cache of news stories where they can live in cache for a 
     * defined period and the cached result will be returned, else the injected
     * service is used to resolve the story.  When under load this will save
     * sending huge numbers of requests for the same data.
     */
    public class NewStoryCache : AgedCache<int, HackerNewStory>, INewStoryCache
    {
        private IHackerNewsService _hackerNewsWebService;
        private ILogger<NewStoryCache> _logger;


        public NewStoryCache(ILogger<NewStoryCache> logger, IServiceClock clock, IHackerNewsService hackerNewsWebService) : base(clock, TimeSpan.FromSeconds(60))
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
    }
}


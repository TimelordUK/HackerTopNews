using HackerTopNews.Model;
using HackerTopNews.Services;
using Microsoft.AspNetCore.Mvc;

namespace HackerTopNews.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HackerTopNewsController : ControllerBase
    {
        private readonly ILogger<HackerTopNewsController> _logger;
        private readonly IHackerNewsService _newsService;
        private readonly INewStoryCache _newStoryCache;

        public HackerTopNewsController(ILogger<HackerTopNewsController> logger, IHackerNewsService hackerNewsWebService, INewStoryCache newStoryCache)
        {
            _logger = logger;
            _newsService = hackerNewsWebService;
            _newStoryCache = newStoryCache;
        }

        [HttpGet(Name = "GetTopID")]
        public async Task<IEnumerable<int>> Get()
        {
            _logger.LogInformation("Get");
            return await _newsService.GetTopStories();
        }

        [HttpGet("{id}")]
        public Task<HackerNewStory> Get(int id)
        {
            return _newStoryCache.Get(id);
        }
    }
}

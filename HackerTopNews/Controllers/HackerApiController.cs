using HackerTopNews.Model;
using HackerTopNews.Services.Cache;
using Microsoft.AspNetCore.Mvc;

namespace HackerTopNews.Controllers
{
    /*
     * wrapper over the Hacker API with a cache in place such that repeated calls are not made to the web service.
     */ 
    [ApiController]
    [Route("[controller]")]
    public class HackerApiController : ControllerBase
    {
        private readonly ILogger<HackerTopNewsController> _logger;
        private readonly ITopStoryCache _topStoryCache;
        private readonly INewsStoryCache _newStoryCache;

        public HackerApiController(ILogger<HackerTopNewsController> logger, ITopStoryCache topStoryCache, INewsStoryCache newStoryCache)
        {
            _logger = logger;
            _topStoryCache = topStoryCache;
            _newStoryCache = newStoryCache;
        }

        [HttpGet(Name = "GetAll")]
        public Task<IReadOnlyList<int>> Get()
        {
            return _topStoryCache.Get();
        }

        [HttpGet("{id}")]
        public Task<HackerNewStory> Get(int id)
        {
            _logger.LogInformation($"Get id {id}");
            return _newStoryCache.Get(id);
        }
    }
}

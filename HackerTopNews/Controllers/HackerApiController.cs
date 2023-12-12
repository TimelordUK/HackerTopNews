using HackerTopNews.Model;
using HackerTopNews.Services.Cache;
using Microsoft.AspNetCore.Mvc;

namespace HackerTopNews.Controllers
{
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
            _logger.LogInformation("Get");
            return _topStoryCache.Get();
        }

        [HttpGet("{id}")]
        public Task<HackerNewStory> Get(int id)
        {
            return _newStoryCache.Get(id);
        }
    }
}

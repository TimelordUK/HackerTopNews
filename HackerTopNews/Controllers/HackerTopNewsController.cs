using HackerTopNews.Model;
using HackerTopNews.Services.Cache;
using Microsoft.AspNetCore.Mvc;

namespace HackerTopNews.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HackerTopNewsController : ControllerBase
    {
        private readonly ILogger<HackerTopNewsController> _logger;
        private readonly ITopStoryCache _topStoryCache;
        private readonly INewsStoryCache _newStoryCache;

        public HackerTopNewsController(ILogger<HackerTopNewsController> logger, ITopStoryCache topStoryCache, INewsStoryCache newStoryCache)
        {
            _logger = logger;
            _topStoryCache = topStoryCache;
            _newStoryCache = newStoryCache;
        }

        [HttpGet("{n}")]
        public async Task<List<HackerNewStory>> GetTopScoring(int n)
        {
            _logger.LogInformation("Get");
            var all = await _topStoryCache.Get();
            var inflateTasks = all.Select(_newStoryCache.Get).ToList();
            var stories = await Task.WhenAll(inflateTasks);
            var asList = stories.ToList();
            // sort in descending order
            asList.Sort((s1, s2) => s2.Score.CompareTo(s1.Score));
            var toTake = Math.Min(asList.Count, n);
            return asList.Slice(0,n);
        }
    }
}

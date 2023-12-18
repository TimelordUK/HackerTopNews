using HackerTopNews.Model;
using HackerTopNews.Services;
using HackerTopNews.Services.Cache;
using Microsoft.AspNetCore.Mvc;

namespace HackerTopNews.Controllers
{
    /*
     * The main web service used to fetch top N ranked stories via a cache which is connected to the 
     * web service.  All top stories are resolved and placed in another cache where new entries will be added
     * and old ones will expire over time.
     */ 
     
    [ApiController]
    [Route("[controller]")]
    public class HackerTopNewsController : ControllerBase
    {
        private readonly ILogger<HackerTopNewsController> _logger;
        private readonly IScoreRankedNews _scoreRankedNews;


        public HackerTopNewsController(ILogger<HackerTopNewsController> logger, IScoreRankedNews scoreRankedNews)
        {
            _logger = logger;
            _scoreRankedNews = scoreRankedNews;
        }

        [HttpGet("{n}")]
        public Task<IReadOnlyList<RankedNewsStory>> GetTopScoring(int n)
        {
            _logger.LogInformation($"HackerTopNewsController.GetTopScoring n = {n}");
            var ranked = _scoreRankedNews.GetTopScoring(n);
            return ranked;
        }
    }
}

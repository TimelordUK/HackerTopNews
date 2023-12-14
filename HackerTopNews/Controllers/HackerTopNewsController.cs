using HackerTopNews.Model;
using HackerTopNews.Services;
using HackerTopNews.Services.Cache;
using Microsoft.AspNetCore.Mvc;

namespace HackerTopNews.Controllers
{
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

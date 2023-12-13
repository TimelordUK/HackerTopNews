using HackerTopNews.Controllers;
using HackerTopNews.Model;
using HackerTopNews.Services.Cache;
using System.Diagnostics;


namespace HackerTopNews.Services
{
    public class ScoreRankedNews : IScoreRankedNews
    {
        private readonly ILogger<ScoreRankedNews> _logger;
        private readonly ITopStoryCache _topStoryCache;
        private readonly INewsStoryCache _newStoryCache;

        public ScoreRankedNews(ILogger<ScoreRankedNews> logger, ITopStoryCache topStoryCache, INewsStoryCache newStoryCache)
        {
            _logger = logger;
            _topStoryCache = topStoryCache;
            _newStoryCache = newStoryCache;
        }

        public async Task<List<RankedNewsStory>> GetTopScoring(int n)
        {
            var sw = new Stopwatch();
            sw.Start();
            n = Math.Max(n, 0);
            var all = await _topStoryCache.Get();
            _logger.LogInformation($"GetTopScoring [{n}] topStoryCache returns {all.Count} results");
            var inflateTasks = all.Select(_newStoryCache.Get).ToList();
            var stories = await Task.WhenAll(inflateTasks);
            var asList = stories.ToList();
            // sort in descending order of score
            asList.Sort((s1, s2) => s2.Score.CompareTo(s1.Score));
            var toTake = Math.Min(asList.Count, n);
            var rawStories = asList.Slice(0, toTake);
            sw.Stop();
            _logger.LogInformation($"GetTopScoring [{n}] elapsedMs = {sw.Elapsed.TotalMilliseconds}, scores = {string.Join(",", rawStories.Select(s => s.Score))}");
            // map results to a type used for the waiting controller
            var res = rawStories.Select(r => new RankedNewsStory(r)).ToList();
            return res;
        }
    }
}

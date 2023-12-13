using HackerTopNews.Model;

namespace HackerTopNews.Services
{
    public interface IScoreRankedNews
    {
        Task<List<RankedNewsStory>> GetTopScoring(int n);
    }
}

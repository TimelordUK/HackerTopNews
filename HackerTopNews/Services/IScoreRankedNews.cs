using HackerTopNews.Model;

namespace HackerTopNews.Services
{
    public interface IScoreRankedNews
    {
        Task<IReadOnlyList<RankedNewsStory>> GetTopScoring(int n);
    }
}

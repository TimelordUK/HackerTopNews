using HackerTopNews.Model;

namespace HackerTopNews.Services.Cache
{
    public interface ITopStoryCache
    {
        Task<IReadOnlyList<int>> Get();
        int Count { get; }
    }
}

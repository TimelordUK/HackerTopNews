using HackerTopNews.Model;

namespace HackerTopNews.Services.Cache
{
    public interface INewsStoryCache
    {
        Task<HackerNewStory> Get(int id);
        int Count { get; }
    }
}

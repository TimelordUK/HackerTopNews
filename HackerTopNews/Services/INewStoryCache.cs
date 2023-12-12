using HackerTopNews.Model;

namespace HackerTopNews.Services
{
    public interface INewStoryCache
    {
        Task<HackerNewStory> Get(int id);
        int Count { get; }
    }
}

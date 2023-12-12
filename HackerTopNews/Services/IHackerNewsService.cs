
using HackerTopNews;
using HackerTopNews.Model;

public interface IHackerNewsService
{
    /// <summary>
    /// fetches ID list of top stories, where each unique ID can resolve to a story
    /// </summary>
    /// <returns>a list of int IDs</returns>
    Task<IReadOnlyList<int>> GetTopStories();
    /// <summary>
    /// resolve a specific story
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<HackerNewStory> GetStory(int id);
}
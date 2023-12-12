using HackerTopNews.Model;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestNews.Support
{
    internal class MoqHackerNewsService
    {

        private IReadOnlyDictionary<int, HackerNewStory> IDToStory { get; set; }
        private Mock<IHackerNewsService> MockedNewsService { get; set; }
        private ConcurrentDictionary<int, int> _invocations = new ConcurrentDictionary<int, int>();

        public int GetInvocations(int id)
        {
            return _invocations.TryGetValue(id, out var r) ? r : 0;
        }

        public MoqHackerNewsService()
        {
            IDToStory = MockResponses.Stories.ToDictionary(s => s.Id);
        }
        public IHackerNewsService GetMockedHackerNewsWebService()
        {
            MockedNewsService = new Mock<IHackerNewsService>();
            MockedNewsService.Setup(i => i.GetTopStories())
                .ReturnsAsync(MockResponses.IDs)
                .Callback(() =>
            {
                _invocations.AddOrUpdate(0, 1, (key, old) => old + 1);
            });
            MockedNewsService.Setup(i => i.GetStory(It.IsAny<int>())).Returns<int>(i =>
            {
                if (IDToStory.TryGetValue(i, out var story))
                {
                    _invocations.AddOrUpdate(i, 1, (key, old) => old + 1);
                    return Task.FromResult(story);
                }

                throw new ArgumentOutOfRangeException($"id {i} unknown");
            });
            return MockedNewsService.Object;
        }
    }
}

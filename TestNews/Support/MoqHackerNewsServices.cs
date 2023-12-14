using HackerTopNews.Model;
using Microsoft.AspNetCore.Http;
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
        private IReadOnlyDictionary<int, HackerNewStory> IDToStory { get; }
        private Mock<IHackerNewsService> MockedNewsService { get; }
        private ConcurrentDictionary<int, int> _invocations = new ConcurrentDictionary<int, int>();
        public IHackerNewsService Service => MockedNewsService.Object;

        public int GetInvocations(int id)
        {
            return _invocations.TryGetValue(id, out var r) ? r : 0;
        }

        public int GetInvocations()
        {
            return _invocations.Values.Sum();
        }

        public MoqHackerNewsService()
        {
            IDToStory = MockResponses.Stories.ToDictionary(s => s.Id);
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

                throw new BadHttpRequestException($"id {i} unknown");
            });
        }
    }
}

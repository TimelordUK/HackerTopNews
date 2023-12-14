using HackerTopNews;
using HackerTopNews.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TestNews.Support;

namespace TestNews
{
    [TestFixture]
    public class HackerNewsServiceTest
    {
        private IHackerNewsService _service;

        [SetUp]
        public void Setup()
        {
            var builder = TestAppBuilder.Make();
            var s = builder.Build().Services;
            _service = s.GetRequiredService<IHackerNewsService>();
        }

        [Test]
        public async Task Test_Fetch_Top_IDs()
        {
            var res = await _service.GetTopStories();
            Assert.That(res, Is.Not.Null);
            Assert.That(res.Count, Is.AtLeast(10));
        }

        [Test]
        public async Task Test_Single_Id()
        {
            var res = await _service.GetTopStories();
            Assert.That(res, Is.Not.Null);
            Assert.That(res.Count, Is.AtLeast(10));
            var t0 = await _service.GetStory(res[0]);
            Assert.That(t0, Is.Not.Null);
            Assert.That(t0.Id, Is.GreaterThan(0));
            Assert.That(t0.Url, Is.Not.Null);
            Assert.That(t0.By, Is.Not.Null);
            Assert.That(t0.Title, Is.Not.Null);
        }

        [Test]
        [TestCase(100)]
        public async Task Test_Get_Batch_Serialize_Json(int number)
        {
            var res = await _service.GetTopStories();
            Assert.That(res, Is.Not.Null);
            var all = res.Take(number).Select(i => _service.GetStory(i)).ToList();
            var stories = await Task.WhenAll(all);
            Assert.That(stories.Length, Is.EqualTo(number));
            var json = JsonSerializer.Serialize(stories);
            Assert.That(json.Length, Is.GreaterThan(1));
        }

        [Test]
        [TestCase(int.MinValue)]
        public void Test_Fetch_Unknown_Story(int id)
        {
            var exception = Assert.ThrowsAsync<BadHttpRequestException>(() =>
            {
                return _service.GetStory(id);
            });
        }
    }
}
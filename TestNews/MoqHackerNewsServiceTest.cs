using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNews.Support;

namespace TestNews
{
    /*
     * Test the mocked news service to enusure it behaves as if we are calling the web version
     * note data used is first 100 reords of a given invocatoin from to real feel so mocked
     * will be interchanchable.
     */
    [TestFixture]
    internal class MoqHackerNewsServiceTest
    {
        private IHackerNewsService _service;
        private MoqHackerNewsService moqHackerNewsService;
        [SetUp]
        public void Init()
        {
            var builder = TestAppBuilder.MakeWithMoqService(out moqHackerNewsService);
            var s = builder.Build().Services;
            _service = s.GetRequiredService<IHackerNewsService>();
        }
        [Test]
        public async Task Test_Get_All_Stories()
        {
            var all = await _service.GetTopStories();
            Assert.That(all, Is.Not.Null);
            Assert.That(all.Count, Is.EqualTo(MockResponses.IDs.Count));
        }

        [Test]
        public async Task Test_Get_First_Story()
        {
            Assert.That(moqHackerNewsService.GetInvocations(0), Is.EqualTo(0));
            var all = await _service.GetTopStories();
            var first = all[0];
            Assert.That(moqHackerNewsService.GetInvocations(0), Is.EqualTo(1));
            Assert.That(moqHackerNewsService.GetInvocations(first), Is.EqualTo(0));
            Assert.That(all, Is.Not.Null);
            Assert.That(all.Count, Is.EqualTo(MockResponses.IDs.Count));
            var t0 = await _service.GetStory(first);
            Assert.That(t0, Is.Not.Null);
            Assert.That(t0.Id, Is.EqualTo(first));
            Assert.That(moqHackerNewsService.GetInvocations(first), Is.EqualTo(1));
            var t1 = await _service.GetStory(first);
            Assert.That(t1, Is.Not.Null);
            Assert.That(t1.Id, Is.EqualTo(first));
            Assert.That(moqHackerNewsService.GetInvocations(first), Is.EqualTo(2));
        }

        // fetch all ids and then resolve all to receive list of stories
        [Test]
        public async Task Test_Resolve_All_Stories()
        {
            Assert.That(moqHackerNewsService.GetInvocations(0), Is.EqualTo(0));
            var ids = await _service.GetTopStories();
            var allTasks = ids.Select(_service.GetStory).ToArray();
            var stories = await Task.WhenAll(allTasks);
            Assert.That(stories.Count, Is.EqualTo(MockResponses.IDs.Count));
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

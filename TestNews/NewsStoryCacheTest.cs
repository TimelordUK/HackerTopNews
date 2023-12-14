using HackerTopNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TestNews.Support;
using HackerTopNews.Services.Cache;
using HackerTopNews.Services.Clock;

namespace TestNews
{
    [TestFixture]
    internal class NewStoryCacheTest
    {
        private INewsStoryCache _newsCache;
        private IServiceClock _clock;
        private IHackerNewsService _newsService;
        private MoqHackerNewsService _moqHackerNewsService;

        [SetUp]
        public void Setup()
        {
            var builder = TestAppBuilder.MakeWithMoqService(out _moqHackerNewsService);
            var s = builder.Build().Services;
            _newsCache = s.GetRequiredService<INewsStoryCache>();
            _clock = s.GetRequiredService<IServiceClock>();
            _newsService = s.GetRequiredService<IHackerNewsService>();
        }

        [Test]
        public async Task Test_Cache_Fetch_Story_Via_Moq_Service()
        {
            Assert.That(_newsCache.Count, Is.EqualTo(0));
            var res = await _newsService.GetTopStories();
            Assert.That(res, Is.Not.Null);
            Assert.That(res.Count, Is.AtLeast(10));
            var first = res[0];

            // at this point should not have called for this id
            Assert.That(_moqHackerNewsService.GetInvocations(first), Is.EqualTo(0));
            var s0 = await _newsCache.Get(first);
            Assert.That(s0, Is.Not.Null);
            Assert.That(_newsCache.Count, Is.EqualTo(1));
            Assert.That(_moqHackerNewsService.GetInvocations(first), Is.EqualTo(1));
            // fetch same story it should be cached
            var s1 = await _newsCache.Get(first);
            Assert.That(s1, Is.Not.Null);
            Assert.That(_newsCache.Count, Is.EqualTo(1));
            // only 1 call made as result wS Cached
            Assert.That(_moqHackerNewsService.GetInvocations(first), Is.EqualTo(1));
        }

        // if the clock is moved forward then expect the cache to expire and another
        // call is made to the web service.
        [Test]
        public async Task Test_Cache_Expiry_Via_Moq_Service()
        {
            Assert.That(_newsCache.Count, Is.EqualTo(0));
            var res = await _newsService.GetTopStories();
            Assert.That(res, Is.Not.Null);
            Assert.That(res.Count, Is.AtLeast(10));

            var first = res[0];
            // at this point should not have called for this id
            Assert.That(_moqHackerNewsService.GetInvocations(first), Is.EqualTo(0));
            var s0 = await _newsCache.Get(first);
            Assert.That(s0, Is.Not.Null);
            Assert.That(_newsCache.Count, Is.EqualTo(1));
            Assert.That(_moqHackerNewsService.GetInvocations(first), Is.EqualTo(1));
           
            // move clock forwards
            _clock.CurrentTime = _clock.CurrentTime.AddMinutes(5);  
            // fetch same story it should be cached
            var s1 = await _newsCache.Get(first);
            Assert.That(s1, Is.Not.Null);
            Assert.That(_newsCache.Count, Is.EqualTo(1));
            // only 1 call made as result is cached
            Assert.That(_moqHackerNewsService.GetInvocations(first), Is.EqualTo(2));

            _clock.CurrentTime = _clock.CurrentTime.AddSeconds(30);
            s1 = await _newsCache.Get(first);
            Assert.That(s1, Is.Not.Null);
            Assert.That(_moqHackerNewsService.GetInvocations(first), Is.EqualTo(2));
        }
    }
}

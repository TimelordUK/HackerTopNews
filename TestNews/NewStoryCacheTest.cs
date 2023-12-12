using HackerTopNews.Services;
using HackerTopNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TestNews.Support;

namespace TestNews
{
    [TestFixture]
    internal class NewStoryCacheTest
    {
        private INewStoryCache _newsCache;
        private IServiceClock _clock;
        private IHackerNewsService _newsService;
        private MoqHackerNewsService _moqHackerNewsService;

        [SetUp]
        public void Setup()
        {
            _moqHackerNewsService = new MoqHackerNewsService();
            var builder = TestAppBuilder.Make();
            builder.Services.AddSingleton(_moqHackerNewsService.GetMockedHackerNewsWebService()); 
            var s = builder.Build().Services;
            _newsCache = s.GetRequiredService<INewStoryCache>();
            _clock = s.GetRequiredService<IServiceClock>();
            _newsService = s.GetRequiredService<IHackerNewsService>();
        }

        [Test]
        public async Task TestCacheFetchStoryViaMoqService()
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
        public async Task TestCacheExpiryViaMoqService()
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

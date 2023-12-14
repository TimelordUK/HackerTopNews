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
    internal class TopStoryCacheTest
    {
        private ITopStoryCache _storyCache;
        private IServiceClock _clock;
        private MoqHackerNewsService _moqHackerNewsService;

        [SetUp]
        public void Setup()
        {
            _moqHackerNewsService = new MoqHackerNewsService();
            var builder = TestAppBuilder.Make();
            builder.Services.AddSingleton(_moqHackerNewsService.Service); 
            var s = builder.Build().Services;
            _storyCache = s.GetRequiredService<ITopStoryCache>();
            _clock = s.GetRequiredService<IServiceClock>();
        }

        [Test]
        public async Task Test_Cache_Fetch_Ids_Via_MoqService()
        {
            Assert.That(_storyCache.Count, Is.EqualTo(0));
            var res = await _storyCache.Get();
            Assert.That(res, Is.Not.Null);
            Assert.That(res.Count, Is.EqualTo(MockResponses.IDs.Count));
            Assert.That(_moqHackerNewsService.GetInvocations(0), Is.EqualTo(1));
        }
       
        /*
         * given clock has not moved (test clock) tehn do not expect cache to be flused 
         * and no additional call made to the web service.`
         */

        [Test]
        public async Task Test_Cache_Fetch_No_Expire_Fetch()
        {
            Assert.That(_storyCache.Count, Is.EqualTo(0));
            var res = await _storyCache.Get();
            Assert.That(res, Is.Not.Null);
            Assert.That(res.Count, Is.EqualTo(MockResponses.IDs.Count));
            Assert.That(_moqHackerNewsService.GetInvocations(0), Is.EqualTo(1));
            Assert.That(_storyCache.Count, Is.EqualTo(1));
            res = await _storyCache.Get();
            Assert.That(res, Is.Not.Null);
            Assert.That(_moqHackerNewsService.GetInvocations(0), Is.EqualTo(1));
        }

        /*
         * move test clock to flush cache and hence expect another call on service 
         */
        [Test]
        public async Task Test_Cache_Fetch_Expire_Fetch_MoqService()
        {
            Assert.That(_storyCache.Count, Is.EqualTo(0));
            var res = await _storyCache.Get();
            Assert.That(res, Is.Not.Null);
            Assert.That(res.Count, Is.EqualTo(MockResponses.IDs.Count));
            Assert.That(_moqHackerNewsService.GetInvocations(0), Is.EqualTo(1));

            _clock.CurrentTime = _clock.CurrentTime.AddMinutes(1);
            res = await _storyCache.Get();
            Assert.That(res, Is.Not.Null);
            Assert.That(_moqHackerNewsService.GetInvocations(0), Is.EqualTo(2));
        }
    }
}

using HackerTopNews.Controllers;
using HackerTopNews.Services;
using HackerTopNews.Services.Cache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNews.Support;

namespace TestNews
{
    /*
     * the main controller used to resolve all stories from top lost and order them by score and return the
     * top N - only needs to forward calls to IScoreRankedNews
     */
    [TestFixture]
    internal class RankedNewControllerTest
    {
        private MoqHackerNewsService moqHackerNewsService;
        private HackerTopNewsController _controller;

        [SetUp]
        public void Init()
        {
            var builder = TestAppBuilder.MakeWithMoqService(out moqHackerNewsService);
            var s = builder.Build().Services;
            var logger = s.GetRequiredService<ILogger<HackerTopNewsController>>();
            var ranked = s.GetRequiredService<IScoreRankedNews>();
            _controller = new HackerTopNewsController(logger, ranked);
        }

        [Test]
        [TestCase(10, 10)]
        [TestCase(-1, 0)]
        [TestCase(int.MaxValue, 100)]
        public async Task Fetch_Ranked_News_Controller_Test(int items, int expected)
        {
            var res = await _controller.GetTopScoring(items);
            Assert.IsNotNull(res);
            Assert.That(res.Count, Is.EqualTo(expected));  
        }

        [Test]
        [TestCase(50)]
        public async Task Fetch_Stress_No_Time_Change_Test(int items)
        {
            for (var i = 0; i < 100; i++)
            {
                var res = await _controller.GetTopScoring(items);
                Assert.IsNotNull(res);
                Assert.That(res.Count, Is.EqualTo(items));
            }
            var allCalls = moqHackerNewsService.GetInvocations();
            Assert.That(allCalls, Is.EqualTo(MockResponses.IDs.Count + 1));
        }
    }
}

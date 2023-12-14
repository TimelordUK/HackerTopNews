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
    [TestFixture]
    internal class RankedNewControllerTest
    {
        private IHackerNewsService _service;
        private MoqHackerNewsService moqHackerNewsService;
        private HackerTopNewsController _controller;

        [SetUp]
        public void Init()
        {
            var builder = TestAppBuilder.MakeWithMoqService(out moqHackerNewsService);
            var s = builder.Build().Services;
            _service = s.GetRequiredService<IHackerNewsService>();
            var logger = s.GetRequiredService<ILogger<HackerTopNewsController>>();
            var ranked = s.GetRequiredService<IScoreRankedNews>();
            _controller = new HackerTopNewsController(logger, ranked);
        }

        [Test]
        [TestCase(10)]
        public async Task Fetch_Ranked_News_Controller_Test(int items)
        {
            var res = await _controller.GetTopScoring(items);
            Assert.IsNotNull(res);
            Assert.That(res.Count, Is.EqualTo(items));  
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

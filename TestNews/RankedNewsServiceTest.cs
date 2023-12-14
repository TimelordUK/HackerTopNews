using HackerTopNews.Model;
using HackerTopNews.Services;
using HackerTopNews.Services.Cache;
using HackerTopNews.Services.Clock;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNews.Support;

namespace TestNews
{
    [TestFixture]
    internal class RankedNewsServiceTest
    {
        private IScoreRankedNews _rankedNewsService;

        [SetUp]
        public void Setup()
        {
            var builder = TestAppBuilder.MakeWithMoqService(out _);
            var s = builder.Build().Services;
            _rankedNewsService = s.GetRequiredService<IScoreRankedNews>();
        }

        [Test]
        [TestCase(1,1)]
        [TestCase(0, 0)]
        [TestCase(-1, 0)]
        [TestCase(10, 10)]
        [TestCase(100, 100)]
        [TestCase(1000, 100)]
        public async Task TestFetch(int n, int expected)
        {
            var res = await _rankedNewsService.GetTopScoring(n);
            Assert.That(res, Is.Not.Null);
            Assert.That(res.Count, Is.EqualTo(expected));
            CheckOrder(res);
        }

        private void CheckOrder(IReadOnlyList<RankedNewsStory> rankedNewsStories)
        {
            for (var i = 1; i < rankedNewsStories.Count; ++i)
            {
                Assert.That(rankedNewsStories[i].Score, Is.LessThanOrEqualTo(rankedNewsStories[i - 1].Score));
            }
        }
    }
}

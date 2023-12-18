using HackerTopNews;
using HackerTopNews.Services.Clock;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNews.Support
{
    /*
     * use the test clock allowing time to be set for cache expiry and optionally use
     * the mock web service for fetching hacker stories.
     */ 
    internal static class TestAppBuilder
    {
        public static WebApplicationBuilder Make()
        {
            var builder = AppBuilder.Make([]);
            builder.Services.AddSingleton<IServiceClock, TestClock>();
            return builder;
        }

        public static WebApplicationBuilder MakeWithMoqService(out MoqHackerNewsService moqHackerNewsService)
        {
            moqHackerNewsService = new MoqHackerNewsService();
            var builder = Make();
            builder.Services.AddSingleton(moqHackerNewsService.Service);
            return builder; 
        }
    }
}

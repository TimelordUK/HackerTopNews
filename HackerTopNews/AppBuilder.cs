using HackerTopNews.Services;
using HackerTopNews.Services.Cache;
using HackerTopNews.Services.Clock;

namespace HackerTopNews
{
    public static class AppBuilder
    {
        public static WebApplicationBuilder Make(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient<IHackerNewsService, HackerNewsWebService>();
            builder.Services.AddSingleton<INewsStoryCache, NewsStoryCache>();
            builder.Services.AddSingleton<ITopStoryCache, TopStoryCache>();
            builder.Services.AddSingleton<IScoreRankedNews, ScoreRankedNews>();
          
            return builder;
        }

        public static WebApplicationBuilder MakeReal(string[] args)
        {
            WebApplicationBuilder builder = Make(args);
            builder.Services.AddSingleton<IServiceClock, ServiceClock>();
            return builder;
        }
    }
}

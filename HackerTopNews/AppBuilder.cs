﻿using HackerTopNews.Services;

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
            builder.Services.AddSingleton<INewStoryCache, NewStoryCache>();
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
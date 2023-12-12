﻿using HackerTopNews;
using HackerTopNews.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNews
{
    internal static class TestAppBuilder
    {
        public static WebApplicationBuilder Make()
        {
            var builder = AppBuilder.Make([]);
            builder.Services.AddSingleton<IServiceClock, TestClock>();
            return builder;
        }
    }
}
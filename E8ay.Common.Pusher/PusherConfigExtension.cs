using E8ay.Common.Pusher.Impl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.Pusher
{
    public static class PusherConfigExtension
    {
        public static void AddPusher(this IServiceCollection services)
        {
            services.Configure<PusherConfigOptions>(options =>
            {
                options.ApiKey = "c139184be93fe08e2bd2";
                options.AppSecret = "19d7beccc670c6b36e9a";
                options.AppId = "530803";
                options.Cluster = "ap1";
            });
            
            services.AddSingleton<IPusherManager, PusherManager>();
        }
    }
}

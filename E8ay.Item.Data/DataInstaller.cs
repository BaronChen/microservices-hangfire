using E8ay.Item.Data.Impl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Item.Data
{
    public static class DataInstaller
    {
        public static void ConfigureServices(IServiceCollection services, string mongoConnectionString)
        {
            services.AddTransient<IAuctionItemRepository, AuctionItemRepository>();

        }
    }
}

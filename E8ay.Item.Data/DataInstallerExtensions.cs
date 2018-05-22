using E8ay.Item.Data.Impl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Item.Data
{
    public static class DataInstallerExtension
    {
        public static void AddDataLayer(this IServiceCollection services, string mongoConnectionString)
        {
            services.AddTransient<IAuctionItemRepository, AuctionItemRepository>();

        }
    }
}

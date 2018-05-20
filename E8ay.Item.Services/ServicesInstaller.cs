using AutoMapper;
using E8ay.Item.Data;
using E8ay.Item.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Item.Services
{
    public static class ServicesInstaller
    {
        public static void ConfigureServices(IServiceCollection services, string mongoConnectionString)
        {
            services.AddAutoMapper();
            services.AddTransient<IItemService, ItemService>();

            DataInstaller.ConfigureServices(services, mongoConnectionString);
        }
    }
}

using E8ay.Common.ApiClient.Impl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.ApiClient
{
    public static class ApiClientConfigExtension
    {
        public static void AddApiClient(this IServiceCollection services)
        {
            services.Configure<ApiClientOptions>(options =>
            {
                options.ItemServiceUrl = "http://e8ay.item.api";
                options.BidServiceUrl = "http://e8ay.bid.api";
                options.UserServiceUrl = "http://e8ay.user.api";
             
            });

            services.AddTransient<IItemServiceApiClient, ItemServiceApiClient>();
        }

    }
}

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
                options.ItemServiceUrl = "http://e8ay.item.api:8200";
                options.BidServiceUrl = "http://e8ay.bid.api:8300";
                options.UserServiceUrl = "http://e8ay.user.api:8100";
             
            });

            services.AddTransient<IItemServiceApiClient, ItemServiceApiClient>();
        }

    }
}

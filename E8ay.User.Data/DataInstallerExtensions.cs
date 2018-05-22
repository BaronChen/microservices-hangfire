using AspNetCore.Identity.Mongo;
using E8ay.User.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.User.Data
{
    public static class DataInstallerExtension
    {
        public static void AddDataLayer(this IServiceCollection services, string mongoConnectionString)
        {
            services.AddMongoIdentityProvider<AppUser, AppRole>(mongoConnectionString, options =>
            {
                options.Password.RequiredLength = 6;

                options.Password.RequireLowercase = false;

                options.Password.RequireUppercase = false;

                options.Password.RequireNonAlphanumeric = false;

                options.Password.RequireDigit = false;

            });

        }
    }
}

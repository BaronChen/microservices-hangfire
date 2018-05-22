using E8ay.User.Data;
using E8ay.User.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.User.Services
{
    public static class ServicesInstallerExtension
    {
        public static void AddServicesLayer(this IServiceCollection services, string mongoConnectionString)
        {
            services.AddTransient<IJwtGenerator, JwtGenerator>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddDataLayer(mongoConnectionString);
        }
    }
}

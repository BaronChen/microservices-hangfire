using Hangfire;
using Hangfire.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire
{
    public static class HangfireConfig
    {

        public static void ConfigureServices(IServiceCollection services, string mongoConnectionString, string database)
        {
            services.AddHangfire(config =>
            {
                config.UseMongoStorage(mongoConnectionString, database);
            });
        }
        
        public static void ConfigureApp(IApplicationBuilder app, IHostingEnvironment env, string serviceQueueName)
        {

            var options = new BackgroundJobServerOptions
            {
                Queues = new[] { serviceQueueName }
            };

            app.UseHangfireServer(options);

            if (env.IsDevelopment())
            {
                app.UseHangfireDashboard("/hangfire", new DashboardOptions
                {
                    Authorization = new[] { new MyAuthorizationFilter() }
                });

                JobStorage.Current?.GetMonitoringApi()?.PurgeJobs();
            }
            
        }
    }
}

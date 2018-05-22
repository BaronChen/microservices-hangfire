using Hangfire;
using Hangfire.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using E8ay.Common.HangFire.EventBus;
using E8ay.Common.HangFire.Impl;

namespace E8ay.Common.HangFire
{
    public static class HangfireConfigExtensions
    {

        public static void AddHangFireServices(this IServiceCollection services, string mongoConnectionString, string database)
        {
            services.AddHangfire(config =>
            {
                config.UseMongoStorage(mongoConnectionString, database);
            });

            services.AddSingleton<IEventHandlerRegistry, EventHandlerRegistry>();

            services.AddSingleton<IEventProcessor, EventProcessor>();

            services.AddTransient<IJobService, JobService>();
        }

        public static void AddEventHandlerInServices<T>(this IServiceCollection services, string eventName, Type handlerType)
        {
            services.AddTransient(handlerType);
        }

        public static void UseHangFireServices(this IApplicationBuilder app, IHostingEnvironment env, string serviceQueueName)
        {

            var options = new BackgroundJobServerOptions
            {
                Queues = new[] { serviceQueueName },
                Activator =  new ContainerJobActivator(app.ApplicationServices)
            };
            
            //After restart a duplicate server will still hanging around base on https://github.com/HangfireIO/Hangfire/issues/889
            app.UseHangfireServer(options);

            if (env.IsDevelopment())
            {
                app.UseHangfireDashboard("/hangfire", new DashboardOptions
                {
                    Authorization = new[] { new MyAuthorizationFilter() }
                });

                JobStorage.Current?.GetMonitoringApi()?.PurgeJobs(serviceQueueName);
            }
            
        }
        
        public static void UseEventHandlerInServices<T>(this IServiceProvider serviceProvider, string eventName, Type handlerType)
        {
            var registry = serviceProvider.GetService<IEventHandlerRegistry>();
            registry.AddHandler<T>(eventName, handlerType);
        }
    }
}

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
using E8ay.Common.Data;
using E8ay.Common.HangFire.EventData;

namespace E8ay.Common.HangFire
{
    public static class HangfireConfigExtensions
    {

        public static void AddHangFireServices(this IServiceCollection services, string mongoConnectionString, string database)
        {
            var serviceProvider = services.BuildServiceProvider();
            var env = serviceProvider.GetService<IHostingEnvironment>();

            if (env.IsDevelopment())
            {
                DatabaseCleaner.ClearDatabase(mongoConnectionString, database);
            } 

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

        public static void UseHangFireServices(this IApplicationBuilder app, IHostingEnvironment env, string[] serviceQueueNames)
        {

            var options = new BackgroundJobServerOptions
            {
                Queues = serviceQueueNames,
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

            }
            
        }
        
        public static void UseEventHandlerInServices<T>(this IServiceProvider serviceProvider, string eventName, Type handlerType) where T : IEventData
        {
            var registry = serviceProvider.GetService<IEventHandlerRegistry>();
            registry.AddHandler<T>(eventName, handlerType);
        }
    }
}

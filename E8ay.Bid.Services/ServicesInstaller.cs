using AutoMapper;
using E8ay.Bid.Data;
using E8ay.Bid.Services.EventHandler;
using E8ay.Common.HangFire;
using E8ay.Common.HangFire.EventModel;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace E8ay.Bid.Services
{
    public class ServicesInstaller
    {
        public static void ConfigureServices(IServiceCollection services, string mongoConnectionString)
        {
            services.AddAutoMapper();

            DataInstaller.ConfigureServices(services, mongoConnectionString);

            RegisterEventHandler(services);
        }

        public static void RegisterEventHandler(IServiceCollection services)
        {
            HangfireConfig.RegisterEventHandlerInService<AuctionEndEventData>(services, AuctionEndEventData.EventName, typeof(AuctionEndEventHandler));
        }

        //This have to be called at ConfigureApp 
        public static void ConfigureEventHandler(IServiceProvider serviceProvider)
        {
            HangfireConfig.UseEventHandler<AuctionEndEventData>(serviceProvider, AuctionEndEventData.EventName, typeof(AuctionEndEventHandler));
        }
    }
}

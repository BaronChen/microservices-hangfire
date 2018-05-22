using AutoMapper;
using E8ay.Bid.Data;
using E8ay.Bid.Services.EventHandler;
using E8ay.Common.HangFire;
using E8ay.Common.HangFire.EventModel;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace E8ay.Bid.Services
{
    public static class ServicesInstallerExtension
    {
        public static void AddServicesLayer(this IServiceCollection services, string mongoConnectionString)
        {
            services.AddAutoMapper();

            services.AddDataLayer(mongoConnectionString);
            
        }

        public static void AddEventHandlers(this IServiceCollection services)
        {
            services.AddEventHandlerInServices<AuctionEndEventData>(AuctionEndEventData.EventName, typeof(AuctionEndEventHandler));
        }

        //This have to be called at ConfigureApp 
        public static void UseEventHandlers(this IServiceProvider serviceProvider)
        {
            serviceProvider.UseEventHandlerInServices<AuctionEndEventData>(AuctionEndEventData.EventName, typeof(AuctionEndEventHandler));
        }
    }
}

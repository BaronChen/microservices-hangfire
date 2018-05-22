using AutoMapper;
using E8ay.Common.HangFire;
using E8ay.Common.HangFire.EventData;
using E8ay.Item.Data;
using E8ay.Item.Services.EventHandler;
using E8ay.Item.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Item.Services
{
    public static class ServicesInstallerExtension
    {
        public static void AddServicesLayer(this IServiceCollection services, string mongoConnectionString)
        {
            services.AddAutoMapper();
            services.AddTransient<IItemService, ItemService>();

            services.AddDataLayer(mongoConnectionString);
        }

        public static void AddEventHandlers(this IServiceCollection services)
        {
            services.AddEventHandlerInServices<BidPlacedForItemEventData>(BidPlacedForItemEventData.EventName, typeof(BidPlacedForItemEventHandler));
            services.AddEventHandlerInServices<AuctionFinaliseEventData>(AuctionFinaliseEventData.EventName, typeof(AuctionFinaliseEventHandler));
        }

        //This have to be called at ConfigureApp 
        public static void UseEventHandlers(this IServiceProvider serviceProvider)
        {
            serviceProvider.UseEventHandlerInServices<BidPlacedForItemEventData>(BidPlacedForItemEventData.EventName, typeof(BidPlacedForItemEventHandler));
            serviceProvider.UseEventHandlerInServices<AuctionFinaliseEventData>(AuctionFinaliseEventData.EventName, typeof(AuctionFinaliseEventHandler));

        }
    }
}

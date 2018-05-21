using E8ay.Common.HangFire.EventBus;
using E8ay.Common.HangFire.EventModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Bid.Services.EventHandler
{
    public class AuctionEndEventHandler : IEventHandler<AuctionEndEventData>
    {

        public void Handle(Event<AuctionEndEventData> e)
        {
            var id = e.Data.ItemId;

            Console.WriteLine("auction end for item: " + id);
        }
    }
}

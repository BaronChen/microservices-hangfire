using E8ay.Common.HangFire.EventBus;
using E8ay.Common.HangFire.EventData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.Bid.Services.EventHandler
{
    public class AuctionEndEventHandler : IEventHandler<AuctionEndEventData>
    {
        private readonly IBidService _bidService;

        public AuctionEndEventHandler(IBidService bidService)
        {
            _bidService = bidService;
        }

        public Task Handle(Event<AuctionEndEventData> e)
        {
            _bidService.FinaliseAuction(e.Data.ItemId);

            return Task.FromResult(0);
        }
    }
}

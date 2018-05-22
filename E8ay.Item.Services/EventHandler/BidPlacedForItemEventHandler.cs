using E8ay.Common.HangFire.EventBus;
using E8ay.Common.HangFire.EventData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.Item.Services.EventHandler
{
    public class BidPlacedForItemEventHandler: IEventHandler<BidPlacedForItemEventData>
    {
        private readonly IItemService _itemService;

        public BidPlacedForItemEventHandler(IItemService itemService)
        {
            _itemService = itemService;
        }
        
        public async Task Handle(Event<BidPlacedForItemEventData> e)
        {
            await _itemService.UpdateAuctionItemBidInfo(e.Data.ItemId, e.Data.BidPrice, e.Data.UserId);
        }
        
    }
}

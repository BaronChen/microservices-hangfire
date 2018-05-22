using E8ay.Common.Enums;
using E8ay.Common.HangFire.EventBus;
using E8ay.Common.HangFire.EventData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.Item.Services.EventHandler
{
    public class AuctionFinaliseEventHandler : IEventHandler<AuctionFinaliseEventData>
    {
        private readonly IItemService _itemServie;

        public AuctionFinaliseEventHandler(IItemService itemServie)
        {
            _itemServie = itemServie;
        }

        public async Task Handle(Event<AuctionFinaliseEventData> e)
        {
            await _itemServie.UpdateAuctionItemBidInfo(e.Data.ItemId, e.Data.HighestPrice, e.Data.HighestBiderId, string.IsNullOrWhiteSpace(e.Data.HighestBiderId) ? ItemStatus.End : ItemStatus.Sold);
        }
    }
}

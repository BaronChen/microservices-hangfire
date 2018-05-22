using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.ViewModels
{
    public class AuctionBidViewModel
    {
        public string UserId { get; set; }

        public string ItemId { get; set; }

        public DateTimeOffset CreatedDateTime { get; set; }

        public decimal BidPrice { get; set; }
    }
}

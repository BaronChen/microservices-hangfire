using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire.EventData
{
    public class AuctionFinaliseEventData:IEventData
    {
        public static string EventName = "AUCTION_FINALISE_EVENT";

        public string ItemId { get; set; }

        public string HighestBiderId { get; set; }

        public decimal HighestPrice { get; set; }
    }
}

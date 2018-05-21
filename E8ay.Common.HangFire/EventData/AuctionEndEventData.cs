using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire.EventModel
{
    public class AuctionEndEventData
    {
        public static string EventName = "AUCTION_END_EVENT";

        public string ItemId { get; set; }
    }
}

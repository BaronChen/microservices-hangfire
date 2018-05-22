using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire.EventData
{
    public class AuctionEndEventData: IEventData
    {
        public static string EventName = "AUCTION_END_EVENT";

        public string ItemId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.HangFire.EventData
{
    public class BidPlacedForItemEventData:IEventData
    {
        public static string EventName = "BID_PLACED_FOR_ITEM";

        public string ItemId { get; set; }
        public string UserId { get; set; }
        public decimal BidPrice { get; set; }

    }
}

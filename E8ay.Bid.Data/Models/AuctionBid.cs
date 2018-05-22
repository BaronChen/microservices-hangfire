using E8ay.Common.Data.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Bid.Data.Models
{
    public class AuctionBid:BaseModel
    {

        public string UserId { get; set; }

        public string ItemId { get; set; }
      
        public DateTimeOffset CreatedDateTime { get; set; }
        
        public decimal BidPrice { get; set; }
    }
}

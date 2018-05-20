using E8ay.Common.Enums;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Item.Data.Models
{
    public class AuctionItem
    {
        [BsonId]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ItemStatus Status { get; set;}

        public DateTimeOffset EndDateTime { get; set; }

        public DateTimeOffset StartDateTime { get; set; }

        public decimal HighestPrice { get; set; }

        public string HighestBiderId { get; set; }
    }
}

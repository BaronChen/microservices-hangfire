using E8ay.Bid.Data.Models;
using E8ay.Common;
using E8ay.Common.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Bid.Data.Impl
{
    internal class AuctionBidRepository : BaseRepository<AuctionBid>, IAuctionBidRepository
    {
        public AuctionBidRepository(IOptions<MongoOptions> options) : base(options, "auctionBids")
        {
        }

        public IList<AuctionBid> GetListByItemId(string itemId)
        {
            return _collection.AsQueryable().Where(x => x.ItemId == itemId).ToList();
        }
    }
}

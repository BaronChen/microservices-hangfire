using E8ay.Bid.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Bid.Services.Impl
{
    internal class BidService: IBidService
    {
        private readonly IAuctionBidRepository _auctionBidRepository;

        public BidService(IAuctionBidRepository auctionBidRepository)
        {
            _auctionBidRepository = auctionBidRepository;
        }
    }
}

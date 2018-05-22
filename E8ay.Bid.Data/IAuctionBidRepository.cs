using E8ay.Bid.Data.Models;
using E8ay.Common.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Bid.Data
{
    public interface IAuctionBidRepository: IBaseRepository<AuctionBid>
    {
        IList<AuctionBid> GetListByItemId(string itemId);
    }
}

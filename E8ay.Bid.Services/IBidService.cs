using E8ay.Common.Models;
using E8ay.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.Bid.Services
{
    public interface IBidService
    {
        Task<ServiceResult> PlaceBid(AuctionBidViewModel bidViewModel, string userId);

        Task DeleteAllBids();

        void FinaliseAuction(string itemId);
    }
}

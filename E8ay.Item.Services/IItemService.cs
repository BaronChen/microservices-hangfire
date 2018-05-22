using E8ay.Common.Enums;
using E8ay.Common.Models;
using E8ay.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.Item.Services
{
    public interface IItemService
    {
        Task CreateAuctionItem(AuctionItemViewModel itemViewModel);

        IEnumerable<AuctionItemViewModel> GetAllAuctionItems();

        Task UpdateAuctionItemBidInfo(string id, decimal price, string highestBiderId, ItemStatus? itemStatus = null);

        Task SeedAuctionItems();

        ServiceResult ValidateItemForBidding(string itemId);
    }
}

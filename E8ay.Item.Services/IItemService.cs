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

        Task SeedAuctionItems();
    }
}

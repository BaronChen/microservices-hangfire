using E8ay.Item.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.Item.Data
{
    public interface IAuctionItemRepository
    {
        AuctionItem GetItemById(string id);

        IEnumerable<AuctionItem> GetAllItems();

        Task CreateItem(AuctionItem item);

        Task UpdateItem(AuctionItem item);

        Task DeleteItem(string id);

        Task DeleteAll();
    }
}

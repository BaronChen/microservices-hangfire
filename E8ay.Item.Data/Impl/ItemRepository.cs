using E8ay.Common;
using E8ay.Common.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using E8ay.Item.Data.Models;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;
using E8ay.Common.Exceptions;

namespace E8ay.Item.Data.Impl
{
    internal class AuctionItemRepository : BaseRepository, IAuctionItemRepository
    {

        private IMongoCollection<AuctionItem> _auctionItems => _db.GetCollection<AuctionItem>("auctionItems");

        public AuctionItemRepository(IOptions<MongoOptions> options):base(options) {

        }

        public AuctionItem GetItemById(string id)
        {
            return _auctionItems.AsQueryable().Where(x => x.Id == id).SingleOrDefault();
        }

        //Unusual thing to do
        public IEnumerable<AuctionItem> GetAllItems()
        {
            return _auctionItems.AsQueryable().ToList();
        }

        public async Task CreateItem(AuctionItem item)
        {
            await _auctionItems.InsertOneAsync(item);
        }

        public async Task UpdateItem(AuctionItem item)
        {
            var updateResult = await _auctionItems.ReplaceOneAsync(x => x.Id == item.Id, item);
            if (!updateResult.IsAcknowledged || updateResult.ModifiedCount == 0)
            {
                throw new MongoUpdateException("Unable to update item");
            }
        }

        public async Task DeleteItem(string id)
        {
            await _auctionItems.DeleteOneAsync(x => x.Id == id);
        }

        //Unusual thing to do
        public async Task DeleteAll()
        {
            await _auctionItems.DeleteManyAsync(x => true);
        }
    }
}

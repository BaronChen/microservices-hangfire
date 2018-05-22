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
    internal class AuctionItemRepository : BaseRepository<AuctionItem>, IAuctionItemRepository
    {   
        public AuctionItemRepository(IOptions<MongoOptions> options):base(options, "auctionItems") {

        }
        
    }
}

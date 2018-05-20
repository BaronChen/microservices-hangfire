using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace E8ay.Common.Data
{
    public abstract class BaseRepository
    {
        protected readonly IMongoDatabase _db;
        public BaseRepository(IOptions<MongoOptions> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _db = client.GetDatabase(options.Value.Database);
        }

    }
}

using E8ay.Common.Data.Models;
using E8ay.Common.Exceptions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using MongoDB.Driver.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E8ay.Common.Data
{
    public abstract class BaseRepository<T>:IBaseRepository<T> where T:BaseModel
    {
        protected readonly IMongoDatabase _db;
        protected readonly string _collectionName;

        protected IMongoCollection<T> _collection => _db.GetCollection<T>(_collectionName);


        public BaseRepository(IOptions<MongoOptions> options, string collectionName)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _db = client.GetDatabase(options.Value.Database);
            _collectionName = collectionName;
        }

        public virtual async Task Create(T data)
        {
            await _collection.InsertOneAsync(data);
        }

        public virtual async Task Delete(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        //Unusual thing to do
        public virtual async Task DeleteAll()
        {
            await _collection.DeleteManyAsync(x => true);
        }

        //Unusual thing to do
        public virtual IEnumerable<T> GetAll()
        {
            return _collection.AsQueryable().ToList();
        }

        public virtual T GetById(string id)
        {
            return _collection.AsQueryable().Where(x => x.Id == id).SingleOrDefault();
        }

        public virtual async Task Update(T data)
        {
            var updateResult = await _collection.ReplaceOneAsync(x => x.Id == data.Id, data);
            if (!updateResult.IsAcknowledged || updateResult.ModifiedCount == 0)
            {
                throw new MongoUpdateException("Unable to update item");
            }
        }
    }
}

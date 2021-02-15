using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Threading.Tasks;
using TennisClub.Api.Models.Mongo;

namespace TennisClub.Api.Services
{
    public abstract class BaseMongoRepository
    {
        protected IMongoDatabase db;

        public BaseMongoRepository(IConfiguration configuration)
        {
            string connectionString = configuration["DatabaseSettings:ConnectionString"];
            string dbName = configuration["DatabaseSettings:DatabaseName"];

            var client = new MongoClient(connectionString);

            db = client.GetDatabase(dbName);
        }

        protected IMongoQueryable<T> Get<T>(string collectionName, string id)
            where T : BaseMongoEntity
        {
            var collection = this.db.GetCollection<T>(collectionName);

            var result = collection.AsQueryable()
                .Where(x => x.Id == id);

            return result;
        }

        protected IMongoQueryable<T> GetAll<T>(string collectionName)
            where T : BaseMongoEntity
        {
            var collection = this.db.GetCollection<T>(collectionName);

            var result = collection.AsQueryable();

            return result;
        }

        protected async Task<string> Insert<T>(string collectionName, T record)
            where T : BaseMongoEntity
        {
            var collection = this.db.GetCollection<T>(collectionName);

            await collection.InsertOneAsync(record);

            return record.Id;
        }


        public async Task Update<T>(string collectionName, string id, T record)
            where T : BaseMongoEntity
        {
            var collection = this.db.GetCollection<T>(collectionName);

            var filterDef = Builders<T>.Filter.Eq(entry => entry.Id, id);


            var updateDefBuilder = Builders<T>.Update;
            UpdateDefinition<T> updateDef = null;

            foreach (var prop in record.GetType().GetProperties())
            {
                if (prop.Name == "Id" || prop.GetValue(record)==null)
                    continue;   
                if (updateDef != null)
                    updateDef = updateDef.Set(prop.Name, prop.GetValue(record));
                else
                    updateDef = updateDefBuilder.Set(prop.Name, prop.GetValue(record));
            }


            var updateResult = await collection.UpdateManyAsync(filterDef, updateDef);

            if(!updateResult.IsAcknowledged)
            {
                throw new Exception("Update failed");
            }

        }

        public async Task Remove<T>(string collectionName, string id)
            where T : BaseMongoEntity
        {
            var collection = this.db.GetCollection<T>(collectionName);

            var result = await collection.DeleteOneAsync(t => t.Id == id);

            if (!result.IsAcknowledged)
            {
                throw new Exception("Remove failed");
            }
        }
    }
}

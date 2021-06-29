using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories
{
    public class MongoDbItemsRepo : IItemsRepo
    {
        private const string databaseName = "catalog";
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> itemsCollection;

        public MongoDbItemsRepo(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemsCollection = database.GetCollection<Item>(collectionName);
        }
        public async Task CreateItemAsync(Item item)
        {
            await itemsCollection.InsertOneAsync(item);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            await itemsCollection.DeleteOneAsync(item => item.Id == id);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            return await itemsCollection.Find(item => item.Id == id).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            // FilterDefinition<Item>.Empty to return everything
            return await itemsCollection.Find(FilterDefinition<Item>.Empty).ToListAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            await itemsCollection.ReplaceOneAsync(existingItem => existingItem.Id == item.Id, item);
        }
    }
}
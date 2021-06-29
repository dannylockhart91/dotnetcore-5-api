using System;
using System.Collections.Generic;
using Catalog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories
{
    public class MongoDbItemsRepo : IInMemItemsRepo
    {
        private const string databaseName = "catalog";
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> itemsCollection;

        public MongoDbItemsRepo(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemsCollection = database.GetCollection<Item>(collectionName);
        }
        public void CreateItem(Item item)
        {
            itemsCollection.InsertOne(item);
        }

        public void DeleteItem(Guid id)
        {
            itemsCollection.DeleteOne(item => item.Id == id);
        }

        public Item GetItem(Guid id)
        {
            return itemsCollection.Find(item => item.Id == id).SingleOrDefault();
        }

        /*
            Pass empty filter to collection.Find() to return all documents in the collection
        */
        public IEnumerable<Item> GetItems()
        {
            return itemsCollection.Find(FilterDefinition<Item>.Empty).ToList();
        }

        public void UpdateItem(Item item)
        {
            itemsCollection.ReplaceOne(existingItem => existingItem.Id == item.Id, item);
        }
    }
}
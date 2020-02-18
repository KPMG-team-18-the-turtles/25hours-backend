using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;

using TwentyFiveHours.API.Models;

namespace TwentyFiveHours.API.Services
{
    public class MongoService<T>
        where T : AbstractModel
    {
        private readonly IMongoCollection<T> _models;

        public MongoService(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            this._models = database.GetCollection<T>(settings.ModelCollectionName);
        }

        public List<T> Get()
            => this._models.Find(target => true).ToList();

        public T Get(string id)
            => this._models.Find(item => item.ID == id).FirstOrDefault();

        public T Create(T target)
        {
            this._models.InsertOne(target);
            return target;
        }

        public void Update(string id, T newModel)
            => this._models.ReplaceOne(item => item.ID == id, newModel);

        public void Remove(string id)
            => this._models.DeleteOne(item => item.ID == id);
    }
}

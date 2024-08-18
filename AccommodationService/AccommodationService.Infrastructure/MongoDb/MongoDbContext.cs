using AccommodationService.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AccommodationService.Infrastructure.MongoDb
{
    public class MongoDbContext
    {
        private  IMongoDatabase _database;
        public IMongoCollection<Accommodation> Accommodations => _database.GetCollection<Accommodation>("Accommodations");
        public IMongoCollection<Amenity> Amenities => _database.GetCollection<Amenity>("Amenity");
        public MongoDbContext(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
            CreateCollectionsAsync();

        }

        private async void CreateCollectionsAsync()
        {
            
            var collectionNames = _database.ListCollectionNames().ToList();

            if (!collectionNames.Contains("Accommodations"))
            {
                _database.CreateCollection("Accommodations");
                var indexKeyDefinition = Builders<Accommodation>.IndexKeys.Geo2DSphere(accommodation => accommodation.Location);
                await Accommodations.Indexes.CreateOneAsync(new CreateIndexModel<Accommodation>(indexKeyDefinition));
            }

            if (!collectionNames.Contains("Amenities"))
            {
                _database.CreateCollection("Amenities");
            }

          
        }

        public async Task InsertAccommodationAsync(Accommodation accommodation)
        {
            await Accommodations.InsertOneAsync(accommodation);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

       

    }
}

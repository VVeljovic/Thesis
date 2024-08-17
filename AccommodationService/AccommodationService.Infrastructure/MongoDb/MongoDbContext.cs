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
        public MongoDbContext(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
           
          
        }

        private void CreateCollections()
        {
            
            var collectionNames = _database.ListCollectionNames().ToList();

            if (!collectionNames.Contains("Accommodations"))
            {
                _database.CreateCollection("Accommodations");
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

        public IMongoCollection<Accommodation> Accommodations => _database.GetCollection<Accommodation>("Accommodations");
        public IMongoCollection<Amenity> Amenities => _database.GetCollection<Amenity>("Amenity");

    }
}

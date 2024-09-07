using AccommodationService.Application.Dtos;
using AccommodationService.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AccommodationService.Infrastructure.MongoDb
{
    public class AccommodationContext
    {
        private  IMongoDatabase _database;
        public IMongoCollection<Accommodation> Accommodations => _database.GetCollection<Accommodation>("Accommodations");
        public IMongoCollection<Amenity> Amenities => _database.GetCollection<Amenity>("Amenity");
        public AccommodationContext(MongoDbSettings settings)
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

        public async Task<AccommodationDto> GetAccommodationByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<Accommodation>.Filter.Eq(accommodation => accommodation.Id, objectId);
            var order = await Accommodations.Find(filter).FirstOrDefaultAsync();
            
            return AccommodationDto.MapAccommodationToDto(order);
        }

        public async Task<IEnumerable<AccommodationDto>> GetAccommodationsAsync(double longitude, double latitude, int pageSize, int pageNumber)
        {
            var filter = Builders<Accommodation>.Filter.NearSphere(
               a => a.Location,
               new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(longitude, latitude)));

            var accommodations = await  Accommodations
                                        .Find(filter)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Limit(pageSize)
                                        .ToListAsync();
            return accommodations.Select(accommodation => AccommodationDto.MapAccommodationToDto(accommodation));
        }

        public async Task<IEnumerable<AccommodationDto>> GetMyAccommodationsAsync(string userId)
        {
            var accommodations = await Accommodations.Find(accommodation => accommodation.UserId == userId)
                                                           .ToListAsync();

            return accommodations.Select(accommodation => AccommodationDto.MapAccommodationToDto(accommodation));

        }

    }
}

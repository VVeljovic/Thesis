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
        private IMongoDatabase _database;
        public IMongoCollection<Accommodation> Accommodations => _database.GetCollection<Accommodation>("Accommodations");
        public IMongoCollection<Amenity> Amenities => _database.GetCollection<Amenity>("Amenities");
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
            await Amenities.InsertOneAsync(accommodation.Amenity);
            var amenityId = accommodation.Amenity.Id;
            accommodation.AmenityId = amenityId;
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
            var accommodation = await Accommodations.Find(filter).FirstOrDefaultAsync();
            if (accommodation != null)
            {
                var accommodationDto = AccommodationDto.MapAccommodationToDto(accommodation);
                var amenity = await Amenities.Find(a => a.Id == accommodation.AmenityId).FirstOrDefaultAsync();
                if (amenity != null)
                {
                    accommodationDto.Amenity = AmenityDto.MapToAmenityDto(amenity);
                }
                if (accommodation.LastFiveReviews != null && accommodation.LastFiveReviews.Any())
                {
                    accommodationDto.Reviews = accommodation.LastFiveReviews
                        .Select(review => ReviewDto.MapReviewToReviewDto(review))
                        .ToList();
                }

                return accommodationDto;
            };
        
            return null;
        }

        public async Task<IEnumerable<AccommodationDto>> GetAccommodationsAsync(
            double longitude,
            double latitude,
            int pageSize,
            int pageNumber,
            string? address = null,
            DateOnly? checkIn = null,
            DateOnly? checkOut = null,
            int? numberOfGuests = null)
        {
            var filter = Builders<Accommodation>.Filter.NearSphere(
                a => a.Location,
                new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(longitude, latitude))
            );

            if (!string.IsNullOrEmpty(address))
            {
                var addressFilter = Builders<Accommodation>.Filter.Regex(a => a.Address, new MongoDB.Bson.BsonRegularExpression(address, "i"));
                filter = Builders<Accommodation>.Filter.And(filter, addressFilter);
            }
            if (numberOfGuests != null && numberOfGuests != 0)
            {
                var numberOfGuestsFilter = Builders<Accommodation>.Filter.Eq("NumberOfGuests", numberOfGuests);
                filter = Builders<Accommodation>.Filter.And(filter, numberOfGuestsFilter);
            }
            if (checkIn.HasValue && checkOut.HasValue)
            {
                var checkInStart = checkIn.Value.ToDateTime(TimeOnly.MinValue);
                var checkOutEnd = checkOut.Value.ToDateTime(TimeOnly.MaxValue);

                var dateFilter = Builders<Accommodation>.Filter.And(
                    Builders<Accommodation>.Filter.Lte(a => a.AvailableFrom, checkOutEnd),
                    Builders<Accommodation>.Filter.Gte(a => a.AvailableTo, checkInStart)
                );


                filter = Builders<Accommodation>.Filter.And(filter, dateFilter);
            }

            var accommodations = await Accommodations
                .Find(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
            var accommodationIds = accommodations.Select(a => a.AmenityId).Distinct().ToList();
            var amenities = await Amenities.Find(a => accommodationIds.Contains(a.Id)).ToListAsync();
            var amenityDict = amenities.ToDictionary(a => a.Id);
            var accommodationDtos = accommodations.Select(accommodation =>
            {
                var dto = AccommodationDto.MapAccommodationToDto(accommodation);
                if (amenityDict.TryGetValue(accommodation.AmenityId, out var amenity))
                {
                    dto.Amenity = AmenityDto.MapToAmenityDto(amenity);
                }
                return dto;
            });

            return accommodationDtos;
        }

        public async Task<IEnumerable<AccommodationDto>> GetMyAccommodationsAsync(string userId)
        {
            var accommodations = await Accommodations.Find(accommodation => accommodation.UserId == userId)
                                                           .ToListAsync();
            return accommodations.Select(accommodation => AccommodationDto.MapAccommodationToDto(accommodation));
        }

        public async Task UpdateAccommodationAsync(Accommodation updatedAccommodation)
        {
            var filter = Builders<Accommodation>.Filter.Eq(a => a.Id, updatedAccommodation.Id);

            var update = Builders<Accommodation>.Update
                .Set(a => a.PropertyName, updatedAccommodation.PropertyName)
                .Set(a => a.Description, updatedAccommodation.Description)
                .Set(a => a.Address, updatedAccommodation.Address)
                .Set(a => a.PricePerNight, updatedAccommodation.PricePerNight)
                .Set(a => a.NumberOfGuests, updatedAccommodation.NumberOfGuests)
                .Set(a => a.AvailableFrom, updatedAccommodation.AvailableFrom)
                .Set(a => a.AvailableTo, updatedAccommodation.AvailableTo)
                .Set(a => a.Photos, updatedAccommodation.Photos)
                .Set(a => a.UserId, updatedAccommodation.UserId)

                .Set(a => a.Location, updatedAccommodation.Location)
                .Set(a => a.LastFiveReviews, updatedAccommodation.LastFiveReviews);

            await Accommodations.UpdateOneAsync(filter, update);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationService.Domain.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace AccommodationService.Infrastructure.MongoDb
{
    public class ReviewContext
    {
        private IMongoDatabase _database;
        public IMongoCollection<Review> Reviews => _database.GetCollection<Review>("Reviews");
        public IMongoCollection<Accommodation> Accommodations => _database.GetCollection<Accommodation>("Accommodations");

        public ReviewContext(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);

            CreateCollectionAsync();
        }

        private async void CreateCollectionAsync()
        {
            var collectionNames = _database.ListCollectionNames().ToList();
            if (!collectionNames.Contains("Reviews"))
            {
                _database.CreateCollection("Reviews");
            }
        }

        public async Task<Review> CreateReviewAsync(Review review)
        {
            var accommodation = await Accommodations
                .Find(accommodation => accommodation.Id == new ObjectId(review.AccommodationId))
                .FirstOrDefaultAsync();

            if (accommodation != null)
            {
                await Reviews.InsertOneAsync(review);
                accommodation.LastFiveReviews.Insert(0, review);

                var update = Builders<Accommodation>.Update
                .Set(a => a.LastFiveReviews, accommodation.LastFiveReviews);

                await Accommodations.UpdateOneAsync(
                    a => a.Id == accommodation.Id,
                    update);
            }
            return review;
        }
    }
}

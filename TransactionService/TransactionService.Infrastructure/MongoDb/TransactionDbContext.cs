using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.MongoDb
{
    public class TransactionDbContext
    {
        private IMongoDatabase _database;

        public IMongoCollection<Transaction> Transactions => _database.GetCollection<Transaction>("Transactions");

        public TransactionDbContext(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        private async void CreateCollectionAsync()
        {
            var collectionNames = _database.ListCollectionNames().ToList();

            if (!collectionNames.Contains("Transactions"))
            {
                _database.CreateCollection("Transactions");
            }
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            await Transactions.InsertOneAsync(transaction);
            return transaction;
        }

        public async Task UpdateTransactionStatusAsync(string transactionId, string newStatus)
        {
            var filter = Builders<Transaction>.Filter.Eq(a => a.Id , new MongoDB.Bson.ObjectId(transactionId));
            var update = Builders<Transaction>.Update.Set(a => a.Status, newStatus);
            await Transactions.UpdateOneAsync(filter, update);
        }
    }
}

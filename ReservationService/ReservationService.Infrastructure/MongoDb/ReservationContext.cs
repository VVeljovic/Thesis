using AccommodationService.Infrastructure.MongoDb;
using MongoDB.Driver;

public class ReservationContext
{
    public readonly IMongoDatabase _database;

    public IMongoCollection<Reservation> Reservations => _database.GetCollection<Reservation>("Reservations");

    public ReservationContext(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
    }

    private void CreateCollectionsAsync()
    {
        var collectionNames = _database.ListCollectionNames().ToList();

        if (!collectionNames.Contains("Reservations"))
        {
            _database.CreateCollection("Reservations");
        }
    }

    public async Task InsertReservationAsync(Reservation reservation)
    {
        await Reservations.InsertOneAsync(reservation);
    }
}
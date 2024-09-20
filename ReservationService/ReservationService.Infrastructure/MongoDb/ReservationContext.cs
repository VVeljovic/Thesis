using AccommodationService.Application.Dtos;
using AccommodationService.Infrastructure.MongoDb;
using MongoDB.Driver;
using System.Transactions;

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

    public async Task<Reservation> InsertReservationAsync(Reservation reservation)
    {
        await Reservations.InsertOneAsync(reservation);
        return reservation;
    }

    public async Task<Reservation> FindOneAsync(TransactionRequestDto transactionRequestDto)
    {
        var filters = new List<FilterDefinition<Reservation>>();
        filters.Add(Builders<Reservation>.Filter.Eq("AccommodationId", transactionRequestDto.AccommodationId));
        filters.Add(Builders<Reservation>.Filter.Eq("Status", "Success"));
        var dateFilter = Builders<Reservation>.Filter.And(
       Builders<Reservation>.Filter.Gte(r => r.CheckInDate, transactionRequestDto.DateFrom),
       Builders<Reservation>.Filter.Lte(r => r.CheckOutDate, transactionRequestDto.DateTo)
        );
        filters.Add(dateFilter);
        var filter = Builders<Reservation>.Filter.And(filters);

        var reservation = await Reservations.Find(filter).FirstOrDefaultAsync();
        return reservation;

    }
    public async Task<IEnumerable<ReservationDto>> GetReservationsByUserIdAsync(string userId, int pageSize = 5, int pageNumber = 1,string status = "success")
    {
        var filter = Builders<Reservation>.Filter.Eq(reservation =>reservation.UserId, userId);
        var statusFilter = Builders<Reservation>.Filter.Eq(reservation=>reservation.Status, status);
        filter = Builders<Reservation>.Filter.And(filter,statusFilter);
        var reservations = await Reservations.Find(filter).Skip((pageNumber-1)*pageSize).Limit(pageSize).ToListAsync();
        var reservationsDto = reservations.Select(reservation=>ReservationDto.MapReservationToDto(reservation));
        return reservationsDto;  
    }

    public async Task UpdateReservationAsync(string reservationId, string newStatus)
    {
        var filter = Builders<Reservation>.Filter.Eq(a => a.Id, new MongoDB.Bson.ObjectId(reservationId));
        var update = Builders<Reservation>.Update.Set(a => a.Status, newStatus);
        await Reservations.UpdateOneAsync(filter, update);
    }
}
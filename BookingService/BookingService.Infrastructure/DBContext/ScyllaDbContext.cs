using BookingService.Application.Models;
using Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Infrastructure.DBContext
{
    public class ScyllaDbContext
    {
        private readonly Cluster _cluster;

        private readonly ISession _session;
        public ScyllaDbContext(ScyllaDbSettings settings)
        {
            _cluster = Cluster.Builder().AddContactPoint("localhost").WithPort(9042).Build();
            _session = _cluster.Connect("system");
            InitializeKeyspaceAndTables(_session);
        }

        public ISession Session => _session;

        public void InitializeKeyspaceAndTables(ISession session)
        {
            session.Execute("CREATE KEYSPACE IF NOT EXISTS booking_keyspace " +
                            "WITH replication = {'class': 'SimpleStrategy', 'replication_factor': 3};");

            session.Execute("CREATE TABLE IF NOT EXISTS booking_keyspace.bookings (" +
                            "bookingid uuid PRIMARY KEY, " +
                            "checkindate timestamp, " +
                            "checkoutdate timestamp, " +
                            "totalamount decimal, " +
                            "status text, " +
                            "userid uuid, " +
                            "accommodationid uuid);");
        }

        public async Task SaveBookingAsync(Booking booking)
        {
            var query = "INSERT INTO booking_keyspace.bookings (bookingid, checkindate, checkoutdate, totalamount, status, userid, accommodationid) " +
                        "VALUES (?, ?, ?, ?, ?, ?, ?)";

            var statement = _session.Prepare(query).Bind(
                booking.BookingId,
                booking.CheckInDate,
                booking.CheckOutDate,
                booking.TotalAmount,
                booking.Status,
                booking.UserId,
                booking.AccommodationId);

            await _session.ExecuteAsync(statement);
        }

        public async Task<Booking> GetBookingByIdAsync(string bookingId)
        {
            var query = "SELECT bookingid, checkindate, checkoutdate, totalamount, status, userid, accommodationid " +
                        "FROM booking_keyspace.bookings WHERE bookingid = ?";

            var statement = _session.Prepare(query).Bind(bookingId);
            var row = await _session.ExecuteAsync(statement);

            var bookingRow = row.FirstOrDefault();

            if (bookingRow == null)
            {
                return null;
            }

            return new Booking
            {
                BookingId = bookingRow.GetValue<string>("bookingid"),
                CheckInDate = bookingRow.GetValue<DateTime>("checkindate"),
                CheckOutDate = bookingRow.GetValue<DateTime>("checkoutdate"),
                TotalAmount = bookingRow.GetValue<decimal>("totalamount"),
                Status = bookingRow.GetValue<string>("status"),
                UserId = bookingRow.GetValue<string>("userid"),
                AccommodationId = bookingRow.GetValue<string>("accommodationid")
            };
        }
    }
}

using AccomodationService.Models;
using Cassandra; 
namespace AccomodationService.Repository
{
    public class CassandraRepository
    {
        private readonly Cluster _cluster; 
        private readonly Cassandra.ISession _session;
        public CassandraRepository()
        {
            _cluster = Cluster.Builder().AddContactPoints("localhost").Build();
            _session = _cluster.Connect("system");
            createKeySpace("accommodation");
            
        }
        public bool TestConnection()
        {
            try
            {
                var rs = _session.Execute("SELECT now() FROM system.local");
                return rs != null;
            }
            catch
            {
                return false;
            }
        }
        public void createKeySpace(string keySpaceName)
        {
            var createKeyspaceQuery = $@"
                CREATE KEYSPACE IF NOT EXISTS {keySpaceName} 
                WITH REPLICATION = {{
                    'class': 'SimpleStrategy',
                    'replication_factor': 2
                }}";
            _session.Execute(createKeyspaceQuery);
            _session.ChangeKeyspace("accommodation");
            CreateTable();
            Console.WriteLine($"Keyspace {keySpaceName} created successfully.");
        }
        public void CreateTable()
        {
            var createTableQuery = @"
    CREATE TABLE IF NOT EXISTS accommodation_table (
        id UUID PRIMARY KEY,
        property_name TEXT,
        description TEXT,
        address TEXT,
        price_per_night DECIMAL,
        available_from TIMESTAMP,
        available_to TIMESTAMP,
        photos LIST<TEXT>,
        user_id TEXT,
        parking BOOLEAN,
        wifi BOOLEAN,
        pets_allowed BOOLEAN,
        swimming_pool BOOLEAN,
        spa BOOLEAN,
        fitness_centre BOOLEAN,
        non_smoking_rooms BOOLEAN,
        room_service BOOLEAN
    );";

            _session.Execute(createTableQuery);
            Console.WriteLine("Table 'accommodation_table' created successfully.");
        }
        public void DropTable()
        {
            var dropTableQuery = "DROP TABLE IF EXISTS accommodation_table";

           
            _session.Execute(dropTableQuery);
        }
        public void CreateAccommodation(AccommodationModel accommodationModel)
        {
            var insertQuery = @"
    INSERT INTO accommodation_table (
        id, 
        property_name, 
        description, 
        address, 
        price_per_night, 
        available_from, 
        available_to, 
        photos, 
        parking, 
        wifi, 
        pets_allowed, 
        swimming_pool, 
        spa, 
        fitness_centre, 
        non_smoking_rooms, 
        room_service,
        user_id
    ) VALUES (
        ?, 
        ?, 
        ?, 
        ?, 
        ?, 
        ?, 
        ?, 
        ?, 
        ?, 
        ?, 
        ?, 
        ?, 
        ?, 
        ?, 
        ?, 
        ?,
        ?
    )";

            // Pripremi upit
            var preparedStatement = _session.Prepare(insertQuery);

            // Veži vrednosti
            var boundStatement = preparedStatement.Bind(
                accommodationModel.Id,
                accommodationModel.PropertyName,
                accommodationModel.Description,
                accommodationModel.Address,
                accommodationModel.PricePerNight,
                accommodationModel.AvailableFrom,
                accommodationModel.AvailableTo,
                accommodationModel.Photos,
                accommodationModel.Parking,
                accommodationModel.WiFi,
                accommodationModel.PetsAllowed,
                accommodationModel.SwimmingPool,
                accommodationModel.Spa,
                accommodationModel.FitnessCentre,
                accommodationModel.NonSmokingRooms,
                accommodationModel.RoomService,
                accommodationModel.UserId
            );

            
            _session.Execute(boundStatement);
        }
         public void AddColumnToTable(string columnName, string columnType)
        {
            var query = $@"
                        ALTER TABLE accommodation_table 
                        ADD {columnName} {columnType};";
            _session.Execute(query);
        }
    
        public AccommodationModel GetAccomodationById(Guid id)
        {
            var selectQuery = $@"
        SELECT 
            id, 
            property_name, 
            description, 
            address, 
            price_per_night, 
            available_from, 
            available_to, 
            photos, 
            parking, 
            wifi, 
            pets_allowed, 
            swimming_pool, 
            spa, 
            fitness_centre, 
            non_smoking_rooms, 
            room_service
        FROM accommodation_table
        WHERE id = {id}";

            var row = _session.Execute(selectQuery).FirstOrDefault();

            if (row == null)
            {
                return null;
            }

            var accomodationModel = new AccommodationModel
            {
                Id = row.GetValue<Guid>("id"),
                PropertyName = row.GetValue<string>("property_name"),
                Description = row.GetValue<string>("description"),
                Address = row.GetValue<string>("address"),
                PricePerNight = row.GetValue<decimal>("price_per_night"),
                AvailableFrom = row.GetValue<DateTime>("available_from"),
                AvailableTo = row.GetValue<DateTime>("available_to"),
                Photos = row.GetValue<List<string>>("photos"),
                Parking = row.GetValue<bool?>("parking"),
                WiFi = row.GetValue<bool?>("wifi"),
                PetsAllowed = row.GetValue<bool?>("pets_allowed"),
                SwimmingPool = row.GetValue<bool?>("swimming_pool"),
                Spa = row.GetValue<bool?>("spa"),
                FitnessCentre = row.GetValue<bool?>("fitness_centre"),
                NonSmokingRooms = row.GetValue<bool?>("non_smoking_rooms"),
                RoomService = row.GetValue<bool?>("room_service")
            };

            return accomodationModel;
        }

        public IEnumerable<AccommodationModel> GetAccommodations(int pageSize, int pageIndex)
        {
            int offset = pageIndex * pageSize;
            var selectQuery = $@"
        SELECT 
            id, 
            property_name, 
            description, 
            address, 
            price_per_night, 
            available_from, 
            available_to, 
            photos, 
            parking, 
            wifi, 
            pets_allowed, 
            swimming_pool, 
            spa, 
            fitness_centre, 
            non_smoking_rooms, 
            room_service
        FROM accommodation_table
        LIMIT {pageSize}";
            var results = _session.Execute(selectQuery).ToList();
            var paginationResults = results.Skip(offset).Take(pageSize);
            var accommodations = new List <AccommodationModel>();
            foreach(var row in paginationResults)
            {
                var accommodation = new AccommodationModel
                {
                    Id = row.GetValue<Guid>("id"),
                    PropertyName = row.GetValue<string>("property_name"),
                    Description = row.GetValue<string>("description"),
                    Address = row.GetValue<string>("address"),
                    PricePerNight = row.GetValue<decimal>("price_per_night"),
                    AvailableFrom = row.GetValue<DateTime>("available_from"),
                    AvailableTo = row.GetValue<DateTime>("available_to"),
                    Photos = row.GetValue<List<string>>("photos"),
                    Parking = row.GetValue<bool?>("parking"),
                    WiFi = row.GetValue<bool?>("wifi"),
                    PetsAllowed = row.GetValue<bool?>("pets_allowed"),
                    SwimmingPool = row.GetValue<bool?>("swimming_pool"),
                    Spa = row.GetValue<bool?>("spa"),
                    FitnessCentre = row.GetValue<bool?>("fitness_centre"),
                    NonSmokingRooms = row.GetValue<bool?>("non_smoking_rooms"),
                    RoomService = row.GetValue<bool?>("room_service")
                };
                accommodations.Add(accommodation);
            }
            return accommodations;
        }
    }

}

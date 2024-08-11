using AccomodationService.Models;
using AccomodationService.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AccomodationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccommodationController : ControllerBase
    {
        private readonly CassandraRepository cassandraRepository;
        public AccommodationController()
        {

            cassandraRepository = new CassandraRepository();
        }
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            bool isConnected = cassandraRepository.TestConnection();
            if (isConnected)
            {
                return Ok("Connection successful");
            }
            else
            {
                return StatusCode(500, "Connection failed");
            }
        }
        [HttpPost("keySpace")]
        public IActionResult CreateKeySpace()
        {
            cassandraRepository.createKeySpace("accomodation");
            return Ok("Keyspace was successful created");
        }
        [HttpPost("table")]
        public IActionResult CreateTable()
        {
            cassandraRepository.CreateTable();
            return Ok("Table was successful created");
        }
        [HttpPost("createAccommodation")]
        public IActionResult InsertRow([FromBody] AccommodationModel accommodationModel)
        {
            Console.WriteLine("upao");
            cassandraRepository.CreateAccommodation(accommodationModel);
            return Ok("Accomodation was successful created");
        }
        [HttpGet("getAccommodation/{id}")]
        public IActionResult GetAccomodation(Guid id)
        {
            return Ok(cassandraRepository.GetAccomodationById(id));
           
        }
        [HttpDelete]
        public IActionResult DeleteTable()
        {
            cassandraRepository.DropTable();
            return Ok();
        }
        [HttpPut("updateTable/{columnName}/{columnType}")]
        public IActionResult UpdateTable(string columnName, string columnType)
        {
            cassandraRepository.AddColumnToTable(columnName,columnType);
            return Ok();
        }
        [HttpGet("getAccommodations/{pageSize}/{pageIndex}")]
        public IActionResult GetAccommodations(int pageSize, int pageIndex)
        {
            return Ok(cassandraRepository.GetAccommodations(pageSize, pageIndex));
        }
    }
}

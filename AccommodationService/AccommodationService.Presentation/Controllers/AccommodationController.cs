using AccommodationService.Application.Dtos;
using AccommodationService.Application.Interfaces;
using AccommodationService.Domain.Models;
using AccommodationService.Infrastructure.MongoDb;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace AccommodationService.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccommodationController : ControllerBase
    {
        private readonly IAccommodationService _accommodationService;

        public AccommodationController(IAccommodationService accommodationService)
        {
            _accommodationService = accommodationService;
        }

       

        [HttpPost("create-accommodation")]
        public async Task<IActionResult> CreateAccommodation(AccommodationDto accommodationDto)
        {
           
            await _accommodationService.InsertAccommodationAsync(accommodationDto);
            return Ok(accommodationDto);
        }
    }
}

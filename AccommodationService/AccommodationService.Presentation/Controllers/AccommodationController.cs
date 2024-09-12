﻿using AccommodationService.Application.Dtos;
using AccommodationService.Application.Interfaces;
using AccommodationService.Domain.Models;
using AccommodationService.Infrastructure.MongoDb;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace AccommodationService.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccommodationController : ControllerBase
    {
        private readonly IAccommodationService _accommodationService;

        public AccommodationController(IAccommodationService accommodationService)
        {
            _accommodationService = accommodationService;
        }

        [HttpPost("create-accommodation")]
        public async Task<IActionResult> CreateAccommodationAsync([FromBody]AccommodationDto accommodationDto)
        {
            await _accommodationService.InsertAccommodationAsync(accommodationDto);
            return Ok(accommodationDto);
        }

        [HttpGet("get-accommodation-by-id/{id}")]
        public async Task<IActionResult> GetAccommodationByIdAsync(string id)
        {
            var accommodationDto = await _accommodationService.GetAccommodationByIdAsync(id);
            return Ok(accommodationDto);
        }
        [HttpGet("get-accommodations/{longitude}/{latitude}/{pageSize}/{pageNumber}")]
        public async Task<IActionResult> GetAccommodations(double longitude, double latitude, int pageSize, int pageNumber,[FromQuery]string address, [FromQuery]DateOnly checkIn, [FromQuery]DateOnly checkOut)
        {
            var accommodationsDto = await _accommodationService.GetAccommodationsAsync(longitude,latitude,pageSize, pageNumber);
            return Ok(accommodationsDto);
        }

        [HttpGet("get-my-accommodations/{userId}")]
        public async Task<IActionResult> GetMyAccommodationsAsync(string userId)
        {
            var accommodationsDto = await _accommodationService.GetMyAccommodationsAsync(userId);
            return Ok(accommodationsDto);
        }
    }
}

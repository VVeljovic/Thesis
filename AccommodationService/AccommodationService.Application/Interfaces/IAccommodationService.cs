﻿using AccommodationService.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationService.Application.Interfaces
{
    public interface IAccommodationService
    {

        public  Task<AccommodationDto> InsertAccommodationAsync(AccommodationDto accommodationDto);
   
        public void DeleteAccommodation(string id);

        public Task<AccommodationDto> GetAccommodationByIdAsync(string id);

        public Task<IEnumerable<AccommodationDto>> GetAccommodationsAsync(double longitude, double latitude, int pageSize, int pageNumber, string? address = null, DateOnly? checkIn = null,DateOnly? checkOut = null, int? numberOfGuests = null);

        public Task<IEnumerable<AccommodationDto>> GetMyAccommodationsAsync(string userId);

        public Task<ReviewDto> CreateReview(ReviewDto reviewDto);

        public Task HandleMessageAsync<T>(T message, string queueName);

        public Task<AccommodationDto> UpdateAccommodationAsync(AccommodationDto accommodationDto);
    }
}

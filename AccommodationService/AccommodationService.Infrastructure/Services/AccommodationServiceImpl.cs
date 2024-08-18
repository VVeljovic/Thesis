using AccommodationService.Application.Dtos;
using AccommodationService.Application.Interfaces;
using AccommodationService.Domain.Models;
using AccommodationService.Infrastructure.MongoDb;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationService.Infrastructure.Services
{
    public class AccommodationServiceImpl : IAccommodationService
    {
        private readonly MongoDbContext _dbContext;
        public AccommodationServiceImpl(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void DeleteAccommodation(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AccommodationDto> GetAccommodationByIdAsync(string id)
        {
            var accommodation = await _dbContext.Accommodations.
                Find(accommodation => accommodation.Id == id)
                .FirstOrDefaultAsync();

            var accommodationDto = AccommodationDto.MapAccommodationToDto(accommodation);
            return accommodationDto;
        }

        public async Task<IEnumerable<AccommodationDto>> GetAccommodationsAsync(double longitude,double latitude, int pageSize, int pageNumber)
        {
            var filter = Builders<Accommodation>.Filter.NearSphere(
                a=>a.Location,
                new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(longitude, latitude)));

            var accommodations = await _dbContext.Accommodations
                                        .Find(filter)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Limit(pageSize)
                                        .ToListAsync();
            return accommodations.Select(accommodation => AccommodationDto.MapAccommodationToDto(accommodation));
        }

        public async Task<IEnumerable<AccommodationDto>> GetMyAccommodationsAsync(string userId)
        {
            var accommodations = await _dbContext.Accommodations.Find(accommodation => accommodation.UserId == userId)
                                                           .ToListAsync();

            return accommodations.Select(accommodation => AccommodationDto.MapAccommodationToDto(accommodation));

        }

        public async Task<AccommodationDto> InsertAccommodationAsync(AccommodationDto accommodationDto)
        {
            Accommodation accommodation = AccommodationDto.MapDtoToAccommodation(accommodationDto);

            await  _dbContext.InsertAccommodationAsync(accommodation);

            return accommodationDto;
        }

        public AccommodationDto UpdateAccommodation(AccommodationDto accommodationDto)
        {
            throw new NotImplementedException();
        }
    }
}

using AccommodationService.Application.Dtos;
using AccommodationService.Application.Interfaces;
using AccommodationService.Domain.Models;
using AccommodationService.Infrastructure.MongoDb;
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

        public AccommodationDto GetAccommodation(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AccommodationDto> InsertAccommodationAsync(AccommodationDto accommodationDto)
        {
            Accommodation accommodation = AccommodationDto.mapDtoToAccommodation(accommodationDto);

            await  _dbContext.InsertAccommodationAsync(accommodation);

            return accommodationDto;
        }

        public AccommodationDto UpdateAccommodation(AccommodationDto accommodationDto)
        {
            throw new NotImplementedException();
        }
    }
}

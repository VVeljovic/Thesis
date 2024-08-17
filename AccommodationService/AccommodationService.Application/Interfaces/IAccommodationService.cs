using AccommodationService.Application.Dtos;
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
        
        public AccommodationDto UpdateAccommodation(AccommodationDto accommodationDto);

        public void DeleteAccommodation(string id);

        public AccommodationDto GetAccommodation(string id);
    }
}

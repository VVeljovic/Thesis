using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Dtos.KeycloakDtos;

namespace UserService.Application.Interfaces
{
    public interface IKeycloakService
    {
        Task<string> LoginAsync(LoginRequestDto loginRequestDto);

        Task<UserDto> GetUserAsync(string userId);
    }
}

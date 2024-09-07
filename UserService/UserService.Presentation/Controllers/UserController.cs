using Microsoft.AspNetCore.Mvc;
using UserService.Application.Dtos.KeycloakDtos;
using UserService.Application.Interfaces;

namespace UserService.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IKeycloakService _keycloakService;
        public UserController(IKeycloakService keycloakService)
        {
            _keycloakService = keycloakService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> GetToken([FromBody] LoginRequestDto requestDto)
        {
            var message = await _keycloakService.LoginAsync(requestDto);
            return Ok(message);
        }

        [HttpGet("get-user/{userId}")]
        public async Task<IActionResult> GetUserByUsername(string userId)
        {
            var message = await _keycloakService.GetUserAsync(userId);
            return Ok(message);
        }
    }
}

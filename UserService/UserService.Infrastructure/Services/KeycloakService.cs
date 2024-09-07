using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserService.Application.Dtos.KeycloakDtos;
using UserService.Application.Helpers;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Settings;

namespace UserService.Infrastructure.Services
{
    public class KeycloakService : IKeycloakService
    {
        private readonly HttpClient _httpClient;

        private readonly KeycloakSettings _keycloakSettings;

        public KeycloakService(KeycloakSettings keycloakSettings,HttpClient httpClient)
        {
            _keycloakSettings = keycloakSettings;
            _httpClient = httpClient;
        }

        public async Task<string> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var requestData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type",_keycloakSettings.GrantType),
                new KeyValuePair<string, string>("client_id", _keycloakSettings.Resource),
                new KeyValuePair<string, string>("username", loginRequestDto.Username),
                new KeyValuePair<string, string>("password", loginRequestDto.Password),
                new KeyValuePair<string, string>("client_secret", _keycloakSettings.ClientSecret)
            };
            HttpContent httpContent = new FormUrlEncodedContent(requestData);
            Uri uri = new Uri("realms/user-store/protocol/openid-connect/token", UriKind.Relative);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = httpContent
            };
            HttpResponseMessage message = await _httpClient.SendAsync(httpRequestMessage);
            string responseBody = await message.Content.ReadAsStringAsync();
            return responseBody;
        }

        public async Task<UserDto> GetUserAsync(string userId)
        {
            string token = await GetAdminToken();
            Uri uri = new Uri($"admin/realms/user-store/users/{userId}", UriKind.Relative);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            };
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage message = await _httpClient.SendAsync(httpRequestMessage);
            if (message.IsSuccessStatusCode)
            {
                string responseBody = await message.Content.ReadAsStringAsync();
                var deserializeOptions = SerializerHelper.SerializerOptions();
                var userResponse = JsonSerializer.Deserialize<UserDto>(responseBody, deserializeOptions);
                return userResponse ?? null;
            }
            return null;
        }

        private async Task<string> GetAdminToken()
        {
            LoginRequestDto loginRequestDto = new LoginRequestDto("veljko@test.com", "veljko");
            var deserializeOptions = SerializerHelper.SerializerOptions();
            string responseBody = await LoginAsync(loginRequestDto);
            var tokenResponse = JsonSerializer.Deserialize<TokenResponseDto>(responseBody, deserializeOptions);
            return tokenResponse?.AccessToken;
        }
    }
}

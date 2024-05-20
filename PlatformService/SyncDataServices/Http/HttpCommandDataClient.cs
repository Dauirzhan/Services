using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace SyncDataServices.Http{
    public class HttpCommandDataClient : ICommandDataClient
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendPlatformToCommand(PlatformReadDto plat)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(plat),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}", httpContent);

            if(response.IsSuccessStatusCode){
                System.Console.WriteLine("Ok");
            }
            else{
                System.Console.WriteLine("NOT Ok");
            }

        }
    }
}
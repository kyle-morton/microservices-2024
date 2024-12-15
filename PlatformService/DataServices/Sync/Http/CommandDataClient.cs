using System.Text;
using System.Text.Json;
using PlatformService.Models;

namespace PlatformService.DataServices.Sync.Http;

public interface ICommandDataClient 
{
    Task SendPlatformToCommand(PlatformReadModel model);
}

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public HttpCommandDataClient(HttpClient client, IConfiguration config)
    {
        _httpClient = client;    
        _config = config;
    }


    public async Task SendPlatformToCommand(PlatformReadModel model)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(
            _config["CommandService"], 
            httpContent
        );

        if (!response.IsSuccessStatusCode) 
        {
            Console.WriteLine("HttpCommandDataClient: Sync POST to CommandService FAILED");
        }
        
        Console.WriteLine("HttpCommandDataClient: Sync POST to CommandService OK");
    }
}
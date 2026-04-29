using System.Net.Http.Json;

namespace FarmerFactory.Web.Services;

public class ClientResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public interface IClientService
{
    Task<List<ClientResponse>> GetClientsAsync();
}

public class ClientService : IClientService
{
    private readonly HttpClient _httpClient;

    public ClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ClientResponse>> GetClientsAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<ClientResponse>>("api/v1/client");
            return response ?? new List<ClientResponse>();
        }
        catch
        {
            return new List<ClientResponse>();
        }
    }
}

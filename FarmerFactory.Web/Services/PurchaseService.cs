using System.Net.Http.Json;

namespace FarmerFactory.Web.Services;

public class PurchaseRequest
{
    public int ClientId { get; set; }
    public int ProductId { get; set; }
    public int Qty { get; set; }
}

public class PurchaseResponse
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int ProductId { get; set; }
    public int Qty { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
}

public class ApiErrorResponse
{
    public string Message { get; set; } = string.Empty;
}

public interface IPurchaseService
{
    Task<(bool Success, string? Error, PurchaseResponse? Data)> CreatePurchaseAsync(PurchaseRequest request);
    Task<List<PurchaseResponse>> GetPurchasesAsync();
}

public class PurchaseService : IPurchaseService
{
    private readonly HttpClient _httpClient;

    public PurchaseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(bool Success, string? Error, PurchaseResponse? Data)> CreatePurchaseAsync(PurchaseRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/purchase", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PurchaseResponse>();
                return (true, null, result);
            }
            else
            {
                var errorContent = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                var errorMessage = errorContent?.Message ?? "An error occurred";
                return (false, errorMessage, null);
            }
        }
        catch (Exception ex)
        {
            return (false, $"Error: {ex.Message}", null);
        }
    }

    public async Task<List<PurchaseResponse>> GetPurchasesAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<PurchaseResponse>>("api/v1/purchase");
            return result ?? new List<PurchaseResponse>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching purchases: {ex.Message}");
            return new List<PurchaseResponse>();
        }
    }
}
using System.Net.Http.Json;

namespace FarmerFactory.Web.Services;

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int ProductTypeId { get; set; }
    public string ProductType { get; set; } = string.Empty;
}

public interface IProductService
{
    Task<List<ProductResponse>> GetProductsAsync();
}

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProductResponse>> GetProductsAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<ProductResponse>>("api/v1/product");
            return response ?? new List<ProductResponse>();
        }
        catch
        {
            return new List<ProductResponse>();
        }
    }
}

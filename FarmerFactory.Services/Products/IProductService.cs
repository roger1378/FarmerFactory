using FarmerFactory.Common.Entities.Product;

namespace FarmerFactory.Services.Products;

public interface IProductService
{
    Task<IEnumerable<ProductResponse>> GetAsync();
}

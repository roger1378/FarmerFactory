using FarmerFactory.Repositories.Models;

namespace FarmerFactory.Repositories.Products;

public interface IProductsRepository
{
    Task<IEnumerable<Product>> GetAsync();
}

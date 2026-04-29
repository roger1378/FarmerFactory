using FarmerFactory.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmerFactory.Repositories.Products;

public class ProductsRepository : IProductsRepository
{
    private readonly CoreApiDbContext _coreApiDbContext;

    public ProductsRepository(CoreApiDbContext coreApiDbContext)
    {
        _coreApiDbContext = coreApiDbContext;
    }

    public async Task<IEnumerable<Product>> GetAsync()
    {
        return await _coreApiDbContext.Products
            .Include(p => p.ProductType)
            .AsNoTracking()
            .ToListAsync();
    }
}

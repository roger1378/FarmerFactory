using FarmerFactory.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmerFactory.Repositories.Purchases;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly CoreApiDbContext _coreApiDbContext;

    public PurchaseRepository(CoreApiDbContext coreApiDbContext)
    {
        _coreApiDbContext = coreApiDbContext;
    }

    public async Task<IEnumerable<Purchase>> GetAsync()
    {
        return await _coreApiDbContext.Purchases
            .Include(p => p.Client)
            .Include(p => p.Product)
            .ToListAsync();
    }

    public async Task<Purchase> PostAsync(Purchase purchase)
    {
        _coreApiDbContext.Purchases.Add(purchase);
        await _coreApiDbContext.SaveChangesAsync();

        return purchase;
    }

    public async Task<Purchase> GetByTimeAsync(DateTime dateTime, int clientId, int productId)
    {
        return await _coreApiDbContext.Purchases
            .Where(p => p.ClientId == clientId && p.ProductId == productId &&
                p.PurchaseTime >= dateTime)    
            .OrderByDescending(p => p.PurchaseTime)
            .FirstOrDefaultAsync();
    }
}

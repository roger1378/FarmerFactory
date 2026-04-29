using FarmerFactory.Repositories.Models;

namespace FarmerFactory.Repositories.Purchases;

public interface IPurchaseRepository
{
    Task<IEnumerable<Purchase>> GetAsync();

    Task<Purchase> PostAsync(Purchase purchase);

    Task<Purchase> GetByTimeAsync(DateTime dateTime, int clientId, int productId);
}

using FarmerFactory.Common.Entities.Purchase;

namespace FarmerFactory.Services.Purchases;

public interface IPurchaseService
{
    Task<IEnumerable<PurchaseResponse>> GetAsync();

    Task<PurchaseResponse> PostAsync(PurchaseRequest purchaseRequest);
}

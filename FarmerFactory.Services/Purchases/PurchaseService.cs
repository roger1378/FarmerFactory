using AutoMapper;
using FarmerFactory.Common.Entities.Purchase;
using FarmerFactory.Common.Exceptions;
using FarmerFactory.Repositories.Models;
using FarmerFactory.Repositories.Purchases;

namespace FarmerFactory.Services.Purchases;

public class PurchaseService: IPurchaseService
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IMapper _mapper;

    public PurchaseService(IPurchaseRepository purchaseRepository, IMapper mapper)
    {
        _purchaseRepository = purchaseRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PurchaseResponse>> GetAsync()
    {
        return _mapper.Map<IEnumerable<PurchaseResponse>>(
            await _purchaseRepository.GetAsync());
    }

    public async Task<PurchaseResponse> PostAsync(PurchaseRequest purchaseRequest)
    {
        var limitExceeded = await AssertPurchaseExistsAsync(purchaseRequest);
        if (limitExceeded)
        {
            throw new TooManyRequestsException();
        }

        var purchase = _mapper.Map<Purchase>(purchaseRequest);
        purchase.PurchaseTime = DateTime.UtcNow;

        var result = await _purchaseRepository.PostAsync(purchase);

        var response = _mapper.Map<PurchaseResponse>(result);
        return response;
    }

    private async Task<bool> AssertPurchaseExistsAsync(PurchaseRequest purchaseRequest)
    {
        if ((purchaseRequest.Qty) > 1)
        {
            return true;
        }

        DateTime purchaseTime = DateTime.UtcNow.AddMinutes(-1);
        var purchase = await _purchaseRepository.GetByTimeAsync(purchaseTime,
            purchaseRequest.ClientId,
            purchaseRequest.ProductId);

        if (purchase == null)
        {
            return false;
        }

        if ((purchase.Qty + purchaseRequest.Qty) > 1)
        {
            return true;
        }
            
        return false;
    }
}

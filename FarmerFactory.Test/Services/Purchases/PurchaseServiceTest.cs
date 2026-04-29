using AutoMapper;
using FarmerFactory.Common.Entities.Purchase;
using FarmerFactory.Common.Exceptions;
using FarmerFactory.Repositories.Models;
using FarmerFactory.Repositories.Purchases;
using FarmerFactory.Services.Purchases;
using FarmerFactory.Test.Builders;
using Moq;

namespace FarmerFactory.Test.Services.Purchases;

public class PurchaseServiceTest
{
    private Mock<IPurchaseRepository> _repoMock;
    private Mock<IMapper> _mapperMock;
    private PurchaseService _purchaseService;

    [SetUp]
    public void Setup()
    {
        _repoMock = new Mock<IPurchaseRepository>();
        _mapperMock = new Mock<IMapper>();
        _purchaseService = new PurchaseService(_repoMock.Object, _mapperMock.Object);
    }

    [Test]
    public async Task PostAsync_AllowsPurchase_WhenNoPreviousPurchase()
    {
        var request = new PurchaseRequest 
        { 
            ClientId = 1, 
            ProductId = 1, 
            Qty = 1 
        };

        var purchase = PurchaseBuilder.Builder()
            .WithClientId(request.ClientId)
            .WithProductId(request.ProductId)
            .WithQty(1)
            .Build();

        var response = new PurchaseResponse();

        _repoMock.Setup(r => r.GetByTimeAsync(It.IsAny<DateTime>(), request.ClientId, request.ProductId))
            .ReturnsAsync((Purchase)null);

        _mapperMock.Setup(m => m.Map<Purchase>(request)).Returns(purchase);
        _repoMock.Setup(r => r.PostAsync(purchase)).ReturnsAsync(purchase);
        _mapperMock.Setup(m => m.Map<PurchaseResponse>(purchase)).Returns(response);

        var result = await _purchaseService.PostAsync(request);

        Assert.NotNull(result);
        _repoMock.Verify(r => r.PostAsync(purchase), Times.Once);
    }

    [Test]
    public async Task PostAsync_AllowsPurchase_WhenPreviousPurchaseButTotalQtyIsOne()
    {
        var request = new PurchaseRequest
        {
            ClientId = 1,
            ProductId = 1,
            Qty = 1
        };

        var purchase = PurchaseBuilder.Builder()
            .WithClientId(request.ClientId)
            .WithProductId(request.ProductId)
            .WithQty(0)
            .Build();

        var purchase1 = PurchaseBuilder.Builder()
            .WithClientId(request.ClientId)
            .WithProductId(request.ProductId)
            .WithQty(1)
            .Build();

        var response = new PurchaseResponse();

        _repoMock.Setup(r => r.GetByTimeAsync(It.IsAny<DateTime>(), request.ClientId, request.ProductId))
            .ReturnsAsync(purchase);

        _mapperMock.Setup(m => m.Map<Purchase>(request)).Returns(purchase1);
        _repoMock.Setup(r => r.PostAsync(purchase1)).ReturnsAsync(purchase1);
        _mapperMock.Setup(m => m.Map<PurchaseResponse>(purchase1)).Returns(response);

        var result = await _purchaseService.PostAsync(request);

        Assert.NotNull(result);
        _repoMock.Verify(r => r.PostAsync(purchase1), Times.Once);
    }

    [Test]
    public void PostAsync_ThrowsTooManyRequestsException_WhenTotalQtyExceedsOne()
    {
        var request = new PurchaseRequest
        {
            ClientId = 1,
            ProductId = 1,
            Qty = 1
        };

        var previous = PurchaseBuilder.Builder()
            .WithClientId(request.ClientId)
            .WithProductId(request.ProductId)
            .WithQty(1)
            .Build();

        _repoMock.Setup(r => r.GetByTimeAsync(It.IsAny<DateTime>(), request.ClientId, request.ProductId))
            .ReturnsAsync(previous);

        Assert.ThrowsAsync<TooManyRequestsException>(async () => await _purchaseService.PostAsync(request));
        _repoMock.Verify(r => r.PostAsync(It.IsAny<Purchase>()), Times.Never);
    }
}

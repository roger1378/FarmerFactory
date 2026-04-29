using FarmerFactory.Repositories.Purchases;
using FarmerFactory.Test.Builders;
using FarmerFactory.Test.Helpers;

namespace FarmerFactory.Test.Repositories.Purchases;

public class PurchaseRepositoryTest
{
    [Test]
    public async Task PostAsync_AddsPurchaseToDatabase()
    {
        var dbContext = SeededCoreApiDbContext.BuildCoreApiDbContext();

        // Arrange
        var client = ClientBuilder.Builder()
            .WithName("Client 1")
            .Build();

        var product = ProductBuilder.Builder()
            .WithName("Product 1")
            .WithPrice(99.99m)
            .Build();

        dbContext.Clients.Add(client);
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var purchase = PurchaseBuilder.Builder()
            .WithClientId(client.Id)
            .WithProductId(product.Id)
            .WithPurchaseTime(System.DateTime.UtcNow)
            .Build();

        var repository = new PurchaseRepository(dbContext);

        // Act
        var result = await repository.PostAsync(purchase);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(purchase.ClientId, result.ClientId);
        Assert.AreEqual(purchase.ProductId, result.ProductId);
        Assert.AreEqual(purchase.PurchaseTime, result.PurchaseTime);
        Assert.AreNotEqual(0, result.Id);
        Assert.AreEqual(1, dbContext.Purchases.Count());
    }

    [Test]
    public async Task GetByTimeAsync_ReturnsLatestPurchaseWithinTimeWindow()
    {
        var dbContext = SeededCoreApiDbContext.BuildCoreApiDbContext();

        // Arrange
        var client = ClientBuilder.Builder()
            .WithName("Client 2")
            .Build();

        var product = ProductBuilder.Builder()
            .WithName("Corn")
            .WithPrice(10.0m)
            .Build();

        dbContext.Clients.Add(client);
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        var purchaseTime = System.DateTime.UtcNow.AddSeconds(-30);
        var purchase = PurchaseBuilder.Builder()
            .WithClientId(client.Id)
            .WithProductId(product.Id)
            .WithPurchaseTime(purchaseTime)
            .Build();

        dbContext.Purchases.Add(purchase);
        await dbContext.SaveChangesAsync();

        var repository = new PurchaseRepository(dbContext);

        // Act
        var since = System.DateTime.UtcNow.AddMinutes(-1);
        var result = await repository.GetByTimeAsync(since, client.Id, product.Id);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(purchase.ClientId, result.ClientId);
        Assert.AreEqual(purchase.ProductId, result.ProductId);
        Assert.AreEqual(purchase.PurchaseTime, result.PurchaseTime);
    }
}

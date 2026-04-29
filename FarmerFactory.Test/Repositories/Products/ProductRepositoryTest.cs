using FarmerFactory.Repositories.Models;
using FarmerFactory.Repositories.Products;
using FarmerFactory.Test.Builders;
using FarmerFactory.Test.Helpers;

namespace FarmerFactory.Test.Repositories.Products;

public class ProductRepositoryTest
{
    [Test]
    public async Task GetAsync_ReturnsAllCorns()
    {
        // Arrange
        using var dbContext = SeededCoreApiDbContext.BuildCoreApiDbContext();

        var productType1 = ProductTypeBuilder.Builder()
            .WithName("CornType")
            .Build();

        var productType2 = ProductTypeBuilder.Builder()
            .WithName("BeanType")
            .Build();

        var product1 = ProductBuilder.Builder()
            .WithName("Corn 1")
            .WithProductTypeId(productType1.Id)
            .WithProductType(productType1)
            .Build();

        var product2 = ProductBuilder.Builder()
            .WithName("Corn 2")
            .WithProductTypeId(productType2.Id)
            .WithProductType(productType2)
            .Build();

        dbContext.ProductTypes.AddRange(productType1, productType2);
        dbContext.Products.AddRange(product1, product2);
        await dbContext.SaveChangesAsync();

        var repository = new ProductsRepository(dbContext);

        // ACT
        var result = await repository.GetAsync();
        
        // Assert
        Assert.NotNull(result);
        var list = result.ToList();
        Assert.AreEqual(2, list.Count);
        Assert.True(list.Any(c => c.Name == "Corn 1"));
        Assert.True(list.Any(c => c.Name == "Corn 2"));
    }
}

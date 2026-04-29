using FarmerFactory.Repositories.Clients;
using FarmerFactory.Repositories.Models;
using FarmerFactory.Test.Builders;
using FarmerFactory.Test.Helpers;

namespace FarmerFactory.Test.Repositories.Clients;

public class ClientRepositoryTest
{
    [Test]
    public async Task GetAsync_ReturnsAllClients()
    {
        // Arrange
        var client1 = ClientBuilder.Builder()
            .WithName("Client 1")
            .Build();

        var client2 = ClientBuilder.Builder()
            .WithName("Client 2")
            .Build();

        var dbContext = await SeededCoreApiDbContext
            .BuildCoreApiDbContextAsync(new Client[] { client1, client2 });

        var repository = new ClientRepository(dbContext);

        // ACT
        var result = await repository.GetAsync();
        
        // Assert
        Assert.NotNull(result);
        var list = result.ToList();
        Assert.AreEqual(2, list.Count);
        Assert.True(list.Any(c => c.Name == "Client 1"));
        Assert.True(list.Any(c => c.Name == "Client 2"));
    }
}

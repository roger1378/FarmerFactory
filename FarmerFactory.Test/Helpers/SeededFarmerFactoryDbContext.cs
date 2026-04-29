using FarmerFactory.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FarmerFactory.Test.Helpers;

public static class SeededCoreApiDbContext
{
    public static CoreApiDbContext
       BuildCoreApiDbContext()
    {
        var dbContext = new CoreApiDbContext(
            new DbContextOptionsBuilder<CoreApiDbContext>()
                .UseInMemoryDatabase($"FarmerFactoryDb-{Guid.NewGuid():N}")
                .Options);

        return dbContext;
    }

    public static async Task<CoreApiDbContext>
        BuildCoreApiDbContextAsync<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
    {
        var dbContext = BuildCoreApiDbContext();

        await dbContext.Set<TEntity>().AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();

        return dbContext;
    }

    public static (CoreApiDbContext dbContext, string databaseName)
       BuildCoreApiDbContextWithDatabaseName()
    {
        var databaseName = $"FarmerFactoryDb-{Guid.NewGuid():N}";
        var dbContext = new CoreApiDbContext(
            new DbContextOptionsBuilder<CoreApiDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options);

        return (dbContext, databaseName);
    }
}

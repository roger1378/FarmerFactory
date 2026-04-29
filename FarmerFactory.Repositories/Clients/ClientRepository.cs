using FarmerFactory.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmerFactory.Repositories.Clients;

public class ClientRepository : IClientRepository
{
    private readonly CoreApiDbContext _coreApiDbContext;

    public ClientRepository(CoreApiDbContext coreApiDbContext)
    {
        _coreApiDbContext = coreApiDbContext;
    }

    public async Task<IEnumerable<Client>> GetAsync()
    {
        return await _coreApiDbContext.Clients
            .ToListAsync();
    }
}

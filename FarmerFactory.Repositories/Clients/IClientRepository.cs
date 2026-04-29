using FarmerFactory.Repositories.Models;

namespace FarmerFactory.Repositories.Clients;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAsync();
}

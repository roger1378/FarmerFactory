using FarmerFactory.Common.Entities.Client;

namespace FarmerFactory.Services.Clients;

public interface IClientService
{
    Task<IEnumerable<ClientResponse>> GetAsync();
}

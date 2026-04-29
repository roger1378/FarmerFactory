using AutoMapper;
using FarmerFactory.Common.Entities.Client;
using FarmerFactory.Repositories.Clients;

namespace FarmerFactory.Services.Clients;

public class ClientService: IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public ClientService(IClientRepository clientRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClientResponse>> GetAsync()
    {
        return _mapper.Map<IEnumerable<ClientResponse>>(
            await _clientRepository.GetAsync());
    }
}

using AutoMapper;
using FarmerFactory.Common.Entities.Client;
using FarmerFactory.Repositories.Models;

namespace FarmerFactory.Services.Clients.Mapping;

public class ClientMapping: Profile
{
    public ClientMapping()
    {
        CreateMap<Client, ClientResponse>()
            .ReverseMap();
    }
}

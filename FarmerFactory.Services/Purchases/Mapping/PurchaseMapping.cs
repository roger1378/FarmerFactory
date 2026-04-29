using AutoMapper;
using FarmerFactory.Common.Entities.Purchase;
using FarmerFactory.Repositories.Models;

namespace FarmerFactory.Services.Purchases.Mapping;

public class PurchaseMapping : Profile
{
    public PurchaseMapping()
    {
        CreateMap<Purchase, PurchaseResponse>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name))
            .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => src.PurchaseTime))
            .ReverseMap();

        CreateMap<Purchase, PurchaseRequest>()
            .ReverseMap();
    }
}

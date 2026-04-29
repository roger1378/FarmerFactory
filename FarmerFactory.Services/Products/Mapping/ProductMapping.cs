using AutoMapper;
using FarmerFactory.Common.Entities.Product;
using FarmerFactory.Repositories.Models;

namespace FarmerFactory.Services.Products.Mapping;

public class ProductMapping : Profile
{
    public ProductMapping()
    {
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.Name))
            .ReverseMap();
    }
}

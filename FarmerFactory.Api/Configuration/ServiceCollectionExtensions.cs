using AutoMapper;
using FarmerFactory.Repositories.Clients;
using FarmerFactory.Repositories.Products;
using FarmerFactory.Repositories.Purchases;
using FarmerFactory.Services.Clients;
using FarmerFactory.Services.Products;
using FarmerFactory.Services.Purchases;

namespace FarmerFactory.Api.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMapperServices(
        this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddMaps(new[]
        {
            typeof(ClientService).Assembly,
            typeof(ProductService).Assembly,
            typeof(PurchaseService).Assembly
        }));

        return services;
    }

    public static IServiceCollection AddMyDependencyGroup(
         this IServiceCollection services)
    {
        //Repositories
        services.AddTransient<IClientRepository, ClientRepository>();
        services.AddTransient<IProductsRepository, ProductsRepository>();
        services.AddTransient<IPurchaseRepository, PurchaseRepository>();

        //Services
        services.AddTransient<IClientService, ClientService>();
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<IPurchaseService, PurchaseService>();

        return services;
    }
}

using AutoMapper;
using FarmerFactory.Common.Entities.Product;
using FarmerFactory.Repositories.Products;

namespace FarmerFactory.Services.Products;

public class ProductService: IProductService
{
    private readonly IProductsRepository _productsRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductsRepository productsRepository, IMapper mapper)
    {
        _productsRepository = productsRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductResponse>> GetAsync()
    {
        return _mapper.Map<IEnumerable<ProductResponse>>(
            await _productsRepository.GetAsync());
    }
}

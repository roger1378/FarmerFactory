using FarmerFactory.Common.Entities.Product;
using FarmerFactory.Services.Products;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FarmerFactory.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ProductResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(
                await _productService.GetAsync());
        }
    }
}

using FarmerFactory.Common.Entities.Client;
using FarmerFactory.Common.Entities.Purchase;
using FarmerFactory.Common.Exceptions;
using FarmerFactory.Services.Purchases;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FarmerFactory.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(PurchaseResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(TooManyRequestsException), (int)HttpStatusCode.TooManyRequests)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<IActionResult> PostAsync(PurchaseRequest purchaseRequest)
        {
            return Ok(
                await _purchaseService.PostAsync(purchaseRequest));
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(List<PurchaseResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(
                await _purchaseService.GetAsync());
        }
    }
}

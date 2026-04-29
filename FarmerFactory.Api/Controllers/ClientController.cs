using FarmerFactory.Common.Entities.Client;
using FarmerFactory.Services.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FarmerFactory.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ClientResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(
                await _clientService.GetAsync());
        }
    }
}

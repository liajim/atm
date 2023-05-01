using Microsoft.AspNetCore.Mvc;
using AtmCoroBain.Models;
using AtmCoroBain.Services;

namespace AtmCoroBain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService ClientService)
        {
            _clientService = ClientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var Clients = await _clientService.GetClientsAsync();

            if (Clients == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Clients in database");
            }

            return StatusCode(StatusCodes.Status200OK, Clients);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetClient(int id, bool includeAccounts = false)
        {
            Client? Client = await _clientService.GetClientAsync(id, includeAccounts);

            if (Client == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Client found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, Client);
        }

        [HttpPost]
        public async Task<ActionResult<Client>> AddClient(Client Client)
        {
            var dbClient = await _clientService.AddClientAsync(Client);

            if (dbClient == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{Client.Name} could not be added.");
            }

            return CreatedAtAction("GetClient", new { id = Client.Id }, Client);
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateClient(int id, Client Client)
        {
            if (id != Client.Id)
            {
                return BadRequest();
            }

            Client? dbClient = await _clientService.UpdateClientAsync(Client);

            if (dbClient == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{Client.Name} could not be updated");
            }

            return NoContent();
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var Client = await _clientService.GetClientAsync(id, false);
            if (Client == null)
            {
                return BadRequest("Client does not exists");
            }
            (bool status, string message) = await _clientService.DeleteClientAsync(Client);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, Client);
        }
    }
}
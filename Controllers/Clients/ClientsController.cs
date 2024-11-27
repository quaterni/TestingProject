using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestingProject.Models;
using TestingProject.Services.Abstractions.Clients;

namespace TestingProject.Controllers.Clients;
[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IClientsService _clientsService;

    public ClientsController(IClientsService clientsService)
    {
        _clientsService = clientsService;
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetClient(long id, CancellationToken cancellationToken = default) 
    {
        var result = await _clientsService.GetClientByIdAsync(id, cancellationToken);

        if (result.IsFailed && result.Error.Equals(ClientServiceErrors.NotFound))
        {
            return NotFound();
        }

        var client = result.Value;
        return Ok(new ClientResponse(client.ClientId, client.Username, client.SystemId));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateClient(long id,[FromBody] UpdateClientRequest updateClientRequest, CancellationToken cancellationToken = default)
    {
        var result = await _clientsService.UpdateClientAsync(id,
            new ClientUpdateParameters
            {
                SystemId = updateClientRequest.SystemId,
                Username = updateClientRequest.Username,
            },
            cancellationToken);

        if (result.IsFailed && result.Error.Equals(ClientServiceErrors.NotFound))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteClient(long id, CancellationToken cancellationToken = default)
    {
        var result = await _clientsService.DeleteClientAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> AddClientsIfNotExists(AddClientRequest[] clientRequests, CancellationToken cancellationToken = default)
    {
        var clients = clientRequests
            .Select(request => new Client(
                request.Username, 
                request.ClientId, 
                request.SystemId))
            .ToArray();

       await _clientsService.AddClientsIfNotExists(
            clients,
            cancellationToken);

        return NoContent();
    }
}

using TestingProject.Models;
using TestingProject.Shared;

namespace TestingProject.Services.Abstractions.Clients;

public interface IClientsService
{
    Task<Result<Client>> GetClientByIdAsync(long clientId, CancellationToken cancellationToken = default);

    Task<Result> UpdateClientAsync(long clientId, ClientUpdateParameters parameters, CancellationToken cancellationToken = default);

    Task<Result> DeleteClientAsync(long clientId, CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<Client>>> AddClientsIfNotExists(Client[] clients, CancellationToken cancellationToken = default);
}

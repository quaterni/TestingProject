using TestingProject.Models;

namespace TestingProject.Services.Abstractions;

public interface IClientsService
{
    Task<Client?> GetClientByIdAsync(long clientId, CancellationToken cancellationToken = default);

    Task UpdateClientAsync(Client client, CancellationToken cancellationToken = default);

    Task DeleteClientAsync(long clientId, CancellationToken cancellationToken = default);

    Task<int> AddClientsIfNotExists(Client[] clients, CancellationToken cancellationToken = default);
}

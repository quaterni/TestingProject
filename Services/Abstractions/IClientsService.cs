using TestingProject.Models;

namespace TestingProject.Services.Abstractions;

public interface IClientsService
{
    Task<Client?> GetClientByIdAsync(long clientId);

    Task UpdateClientAsync(Client client);

    Task DeleteClientAsync(long clientId);

    Task<int> AddClientsIfNotExists(Client[] clients);
}

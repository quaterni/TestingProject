using Microsoft.EntityFrameworkCore;
using TestingProject.Data;
using TestingProject.Models;
using TestingProject.Services.Abstractions;

namespace TestingProject.Services;

public class ClientService : IClientsService
{
    private readonly ApplicationDbContext _dbContext;

    public ClientService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> AddClientsIfNotExists(Client[] clients, CancellationToken cancellationToken = default)
    {
        var ids = clients.Select(c => c.ClientId).ToArray();

        var existingClients = await _dbContext.Set<Client>()
            .Where(c => ids.Contains(c.ClientId))
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        foreach (var client in clients)
        {
            if (existingClients.Contains(client))
            {
                continue;
            }
            _dbContext.Add(client);
        }

        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteClientAsync(long clientId, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<Client>()
            .Where(c => c.ClientId == clientId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public Task<Client?> GetClientByIdAsync(long clientId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<Client>()
            .Where(c => c.ClientId == clientId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateClientAsync(Client client, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<Client>()
             .Where(c => c.ClientId == client.ClientId)
             .ExecuteUpdateAsync(opt =>
                 opt.SetProperty(c => c.Username, client.Username)
                    .SetProperty(c => c.SystemId, client.SystemId), 
                    cancellationToken);
    }
}

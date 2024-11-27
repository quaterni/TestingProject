using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TestingProject.Data;
using TestingProject.Models;
using TestingProject.Services.Abstractions.Clients;
using TestingProject.Shared;

namespace TestingProject.Services.Clients;

public class ClientService : IClientsService
{
    private readonly ApplicationDbContext _dbContext;

    public ClientService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<IEnumerable<Client>>> AddClientsIfNotExists(Client[] clients, CancellationToken cancellationToken = default)
    {
        List<Client> existingClients = new();
        var ids = clients.Select(c => c.ClientId).ToArray();

        foreach (var client in clients)
        {
            if(await _dbContext.Set<Client>()
                .Where(c=> c.ClientId == client.ClientId)
                .AnyAsync(cancellationToken))
            {
                existingClients.Add(client);
                continue;
            }
            _dbContext.Add(client);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return Result.Success<IEnumerable<Client>>(existingClients);
    }

    // Изначально написал такую реализацию как более оптимальную с точки зрения количества обращений в бд,
    // но по тз: "добавление и сравнение каждого объекта в этом методе должно выполняться асинхронно" -
    // видимо имеется в виду что запрос в бд должен при проверке каждого элемента
    private async Task<Result<IEnumerable<Client>>> AddClientsIfNotExists_Obsolete(Client[] clients, CancellationToken cancellationToken = default)
    {
        List<Client> existingClients = new();
        var ids = clients.Select(c => c.ClientId).ToArray();

        var clientsFromDb = await _dbContext.Set<Client>()
            .Where(c => ids.Contains(c.ClientId))
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        foreach (var client in clients)
        {
            if (clientsFromDb.Contains(client))
            {
                existingClients.Add(client);
                continue;
            }
            _dbContext.Add(client);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success<IEnumerable<Client>>(existingClients);
    }

    public async Task<Result> DeleteClientAsync(long clientId, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<Client>()
            .Where(c => c.ClientId == clientId)
            .ExecuteDeleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<Client>> GetClientByIdAsync(long clientId, CancellationToken cancellationToken = default)
    {
        var client = await _dbContext.Set<Client>()
            .Where(c => c.ClientId == clientId)
            .FirstOrDefaultAsync(cancellationToken);

        if (client == null)
        {
            return Result.Failure<Client>(ClientServiceErrors.NotFound);
        }

        return client;
    }

    public async Task<Result> UpdateClientAsync(long clientId, ClientUpdateParameters parameters, CancellationToken cancellationToken = default)
    {
        var client = await _dbContext.Set<Client>()
            .Where(c => c.ClientId == clientId)
            .FirstOrDefaultAsync(cancellationToken);

        if (client == null) 
        {
            return Result.Failure(ClientServiceErrors.NotFound);
        }

        if(parameters.SystemId != null)
        {
            client.SystemId = parameters.SystemId.Value;
        }
        if (parameters.Username != null)
        {
            client.Username = parameters.Username;
        }

        _dbContext.Update(client);
        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }
}

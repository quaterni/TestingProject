using TestingProject.Shared;

namespace TestingProject.Services.Abstractions.Clients;

public static class ClientServiceErrors
{
    public static Error NotFound => new("ClientService:NotFound", "Client not found");
}

namespace TestingProject.Controllers.Clients;

public sealed record UpdateClientRequest(string? Username, Guid? SystemId);

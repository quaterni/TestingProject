namespace TestingProject.Controllers.Clients;

public sealed record  AddClientRequest(long ClientId, string Username, Guid SystemId);

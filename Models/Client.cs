
namespace TestingProject.Models;

public class Client
{
    public required long ClientId { get; set; }

    public required string Username { get; set; }

    public required Guid SystemId { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Client client && client.ClientId.Equals(ClientId);
    }

    public override int GetHashCode()
    {
        return ClientId.GetHashCode();
    }
}

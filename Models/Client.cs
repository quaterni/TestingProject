
namespace TestingProject.Models;

public class Client
{
    public Client(string username, long clientId, Guid systemId)
    {
        ClientId = clientId;
        SystemId = systemId;
        Username = username;
    }

    public long ClientId { get; }

    public string Username { get; set; }

    public Guid SystemId { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Client client && client.ClientId.Equals(ClientId);
    }

    public override int GetHashCode()
    {
        return ClientId.GetHashCode();
    }
}

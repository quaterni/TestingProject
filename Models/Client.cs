
namespace TestingProject.Models;

public class Client
{
    public required long ClientId { get; set; }

    public required string Username { get; set; }

    public required Guid SystemId { get; set; }
}

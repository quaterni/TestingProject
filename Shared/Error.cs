namespace TestingProject.Shared;

public record Error(string Name, string Description)
{
    public static Error NullValue => new("NullValue", "Value was null");
}

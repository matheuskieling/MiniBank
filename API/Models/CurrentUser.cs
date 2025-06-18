namespace API.Models;

public class CurrentUser
{
    public Guid UserId { get; set; }
    public required string Username { get; set; }
}
namespace Identity.Models;

public class User
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Hash { get; set; }
    public required string Salt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
}
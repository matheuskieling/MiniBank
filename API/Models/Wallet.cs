namespace API.Models;

public class Wallet
{
    public Guid Id { get; set; }
    public long Balance { get; set; } = 0;
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
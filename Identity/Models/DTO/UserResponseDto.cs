namespace Identity.Models.DTO;

public class UserResponseDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
}

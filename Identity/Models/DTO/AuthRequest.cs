using System.ComponentModel.DataAnnotations;

namespace Identity.Models.DTO;

public record AuthRequest(
    
    [Required] 
    [EmailAddress]
    string UserName,
    
    [Required]
    string Password
    
);
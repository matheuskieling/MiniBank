using System.ComponentModel.DataAnnotations;

namespace API.Models.DTO;

public record AddFundsToWalletRequestDto(
    [Required]
    [Range(1, long.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    long Amount
);
using System.ComponentModel.DataAnnotations;

namespace API.Models.DTO;

public record TransactionRequestDto(
    [Required] Guid ReceiverWalletId,
    [Required] long Amount
);
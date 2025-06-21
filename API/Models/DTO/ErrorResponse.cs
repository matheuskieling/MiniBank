namespace API.Models.DTO;

public record ErrorResponse(
    string Message,
    int StatusCode
);
using System.Net;

namespace API.Models.DTO;

public record ErrorResponse(
    string Message = "An unexpected error occurred",
    int StatusCode = (int)HttpStatusCode.InternalServerError
);
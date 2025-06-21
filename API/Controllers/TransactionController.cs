using API.Models;
using API.Models.DTO;
using API.Services;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionController(ITransactionService service) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreateTransaction(TransactionRequestDto request)
    {
        try
        {
            var result = await service.CreateTransaction(request.ReceiverWalletId, request.Amount);
            if (!result.Errors.IsNullOrEmpty())
            {
                return BadRequest(new ErrorResponse(result.Errors.First().ErrorMessage ?? "Unknow error", StatusCodes.Status400BadRequest));
            }
            return Ok(result.Data);
        }
        catch (WalletNotFoundException ex)
        {
            return NotFound(new ErrorResponse(ex.Message, StatusCodes.Status404NotFound));
        }
        catch (Exception ex) when (ex is InsufficientFundsException or UnprocessableEntityException)
        {
            return UnprocessableEntity(new ErrorResponse(ex.Message, StatusCodes.Status422UnprocessableEntity));
        }
    }
}
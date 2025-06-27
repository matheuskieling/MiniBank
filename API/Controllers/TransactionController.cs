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

    /// <summary>
    /// Creates a new transaction between wallets.
    /// </summary>
    /// <param name="request">The transaction request containing the receiver wallet ID and the amount to transfer.</param>
    /// <returns>
    /// Returns an HTTP 200 status code with the transaction details if successful.
    /// Returns an HTTP 400 status code if the request is invalid.
    /// Returns an HTTP 404 status code if the receiver wallet is not found.
    /// Returns an HTTP 422 status code if there are insufficient funds or the transaction cannot be processed.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(Transaction), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
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
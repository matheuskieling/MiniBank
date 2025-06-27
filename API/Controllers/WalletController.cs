using System.Reflection.Metadata.Ecma335;
using API.Models;
using API.Models.DTO;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class WalletController(IWalletService walletService) : ControllerBase
{
    /// <summary>
    /// Retrieves a wallet by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the wallet.</param>
    /// <returns>
    /// Returns an HTTP 200 status code with the wallet details.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Wallet), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid id)
    {
        var wallet = await walletService.GetWalletById(id);
        return Ok(wallet);
    }

    /// <summary>
    /// Retrieves the current user's wallet.
    /// </summary>
    /// <returns>
    /// Returns an HTTP 200 status code with the wallet details.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(Wallet), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUserWallet()
    {
        var wallet = await walletService.GetCurrentUserWallet();
        return Ok(wallet);
    }

    /// <summary>
    /// Retrieves a wallet by the user ID.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>
    /// Returns an HTTP 200 status code with the wallet details.
    /// Returns an HTTP 404 status code if the wallet is not found.
    /// </returns>
    [HttpGet("User/{id}")]
    [ProducesResponseType(typeof(Wallet), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUserId(Guid id)
    {
        var wallet = await walletService.GetWalletByUserId(id);
        if (wallet is null)
        {
            return NotFound(new ErrorResponse("Wallet not found for the specified user.", StatusCodes.Status404NotFound));
        }
        return Ok(wallet);
    }

    /// <summary>
    /// Adds funds to the current user's wallet.
    /// </summary>
    /// <param name="request">The request containing the amount to add.</param>
    /// <returns>
    /// Returns an HTTP 200 status code if the funds are added successfully.
    /// Returns an HTTP 400 status code if the request is invalid.
    /// </returns>
    [HttpPost("Deposit")]
    [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddFunds(AddFundsToWalletRequestDto request)
    {
        try
        {
            var result = await walletService.AddFundsToWallet(request.Amount);
            if (result)
            {
                return Ok("Funds added successfully.");
            }
            throw new Exception("Failed to add funds to the wallet.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponse(ex.Message, StatusCodes.Status400BadRequest));
        }
    }
}
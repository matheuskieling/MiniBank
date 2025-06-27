using System.Reflection.Metadata.Ecma335;
using API.Models.DTO;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class WalletController(IWalletService walletService) : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var wallet = await walletService.GetWalletById(id);
        return Ok(wallet);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCurrentUserWallet()
    {
        var wallet = await walletService.GetCurrentUserWallet();
        return Ok(wallet);
    }
    
    [HttpGet("User/{id}")]
    public async Task<IActionResult> GetByUserId(Guid id)
    {
        var wallet = await walletService.GetWalletByUserId(id);
        if (wallet is null)
        {
            return NotFound(new { Message = "Wallet not found for the specified user." });
        }
        return Ok(wallet);
    }
    
    [HttpPost("Deposit")]
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
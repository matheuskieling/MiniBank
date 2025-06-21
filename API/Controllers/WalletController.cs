using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class WalletController(WalletService walletService) : ControllerBase
{

    [HttpGet("/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var wallet = await walletService.GetWalletById(id);
        return Ok(wallet);
    }
}
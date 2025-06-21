using System.Data;
using API.Services.Interfaces;
using Identity.Models.DTO;
using Identity.Services;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IUserService userService, ITokenService tokenService, IWalletService walletService) : ControllerBase
{

    [HttpPost("Login")]
    public async Task<IActionResult> Login(AuthRequest request)
    {
        var user = await userService.AuthenticateUser(request);
        if (user == null)
        {
            return Unauthorized();
        }
        var token = tokenService.GenerateToken(user);
        return Ok(new
        {
            Id = user.Id,
            Token = token
        });
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(AuthRequest request)
    {
        try
        {
            var commandResult = await userService.CreateUser(request);
            if (!commandResult.Succeded)
            {
                return BadRequest(commandResult.Errors);
            }

            var walletResult = await walletService.CreateWallet(commandResult.Data!.Id);
            if (!walletResult.Succeded)
            {
                await userService.DeleteUserById(commandResult.Data!.Id);
                return BadRequest(walletResult.Errors);
            }
            
            return CreatedAtAction(nameof(Login), new { username = request.UserName }, commandResult.Data);
        }
        catch (DuplicateNameException ex)
        {
            return Conflict("User already taken.");
        }
    }
    
    
}
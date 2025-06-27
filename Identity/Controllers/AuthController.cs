using System.Data;
using API.Models.DTO;
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

    /// <summary>
    /// Logs in a user with the provided credentials.
    /// </summary>
    /// <param name="request">The authentication request containing the user's email and password.</param>
    /// <returns>
    /// Returns an HTTP 200 status code with a token if the login is successful.
    /// Returns an HTTP 401 status code if the credentials are invalid.
    /// </returns>
    [HttpPost("Login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(AuthRequest request)
    {
        var user = await userService.AuthenticateUser(request);
        if (user == null)
        {
            return Unauthorized();
        }
        var token = tokenService.GenerateToken(user);
        return Ok(new LoginResponse(user.Id, token));
    }

    /// <summary>
    /// Registers a new user with the provided credentials.
    /// </summary>
    /// <param name="request">The authentication request containing the user's email and password.</param>
    /// <returns>
    /// Returns an HTTP 201 status code if the registration is successful.
    /// Returns an HTTP 400 status code if the request is invalid.
    /// Returns an HTTP 409 status code if the username is already taken.
    /// </returns>
    [HttpPost("Register")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
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
                return BadRequest(new ErrorResponse(walletResult.Errors.First().ErrorMessage!, StatusCodes.Status400BadRequest));
            }
            
            return CreatedAtAction(nameof(Login), new { username = request.UserName }, new { commandResult.Data });
        }
        catch (DuplicateNameException ex)
        {
            return Conflict(new ErrorResponse("User already taken.", StatusCodes.Status409Conflict));
        }
    }
    
    
}
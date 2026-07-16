using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using ECommerceAPI.Application.DTOs.Account;
using ECommerceAPI.Infrastructure.Identity;
using ECommerceAPI.Infrastructure.Identity.Interfaces;

namespace ECommerceAPI.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var user = new AppUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

        await _userManager.AddToRoleAsync(user, "User");

        var token = await _tokenService.CreateTokenAsync(user);

        return Ok(new TokenResponseDto(user.Email!, user.DisplayName, token));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user is null)
            return Unauthorized();

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result)
            return Unauthorized();

        var token = await _tokenService.CreateTokenAsync(user);

        return Ok(new TokenResponseDto(user.Email!, user.DisplayName, token));
    }

    [HttpGet("current-user")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var user = await _userManager.FindByEmailAsync(email!);

        if (user is null)
            return NotFound();

        return Ok(new UserDto(user.Email!, user.DisplayName));
    }

    [HttpPut("address")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAddress([FromBody] AccountAddressDto address)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var user = await _userManager.FindByEmailAsync(email!);

        if (user is null)
            return NotFound();

        user.Address = new AppUserAddress
        {
            Street = address.Street,
            City = address.City,
            State = address.State,
            ZipCode = address.ZipCode
        };

        await _userManager.UpdateAsync(user);
        return NoContent();
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using webapi_boilerplate.Data;
using webapi_boilerplate.Models;
using webapi_boilerplate.Dtos.Auth;
using webapi_boilerplate.Utils;
using Microsoft.EntityFrameworkCore;

namespace webapi_boilerplate.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly TokenUtils _tokenUtils;

    public AuthController(AppDbContext context, TokenUtils tokenUtils)
    {
        _context = context;
        _tokenUtils = tokenUtils;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
        {
            return BadRequest("Email already exists");
        }

        var user = new User
        {
            Email = registerDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Role = UserRole.Customer
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var response = new RegisterResponseDto
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString()
        };
        return CreatedAtAction(nameof(Register), new { id = user.Id }, response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            return Unauthorized();
        }

        var response = new LoginResponseDto
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString(),
            Token = _tokenUtils.GenerateJwtToken(user)
        };
        return Ok(response);
    }

    [HttpGet("user")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(userId ?? "-1"));
        if (user == null)
        {
            return Unauthorized();
        }

        var response = new RegisterResponseDto
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString()
        };
        return Ok(response);
    }
}
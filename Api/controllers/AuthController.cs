using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Api.Security;
using Domain.Entities;
using System.Security.Claims;
using Infrastructure.Data;
using BCrypt.Net;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly ApplicationDbContext _context;

    public AuthController(JwtService jwtService, ApplicationDbContext context)
    {
        _jwtService = jwtService;
        _context = context;
    }

    // =========================
    // REGISTER
    // =========================
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.Role) ||
            string.IsNullOrWhiteSpace(request.TenantId))
        {
            return BadRequest("Todos los campos son obligatorios");
        }

        var exists = await _context.Users
            .AnyAsync(u => u.Username == request.Username);

        if (exists)
            return BadRequest("El usuario ya existe");

        var user = new User
        {
            Username = request.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role,
            TenantId = request.TenantId
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Usuario registrado correctamente");
    }

    // =========================
    // LOGIN
    // =========================
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null)
            return Unauthorized("Credenciales incorrectas");

        var validPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

        if (!validPassword)
            return Unauthorized("Credenciales incorrectas");

        var token = _jwtService.GenerateToken(user);

        return Ok(new { token });
    }

    // =========================
    // CAMBIO DE CLAVE
    // =========================
    [Authorize]
    [HttpPost("CambioDeClave")]
    public async Task<IActionResult> CambioDeClave([FromBody] ChangePasswordRequest request)
    {
        var username = User.Identity?.Name;

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            return NotFound("Usuario no encontrado");

        var validPassword = BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.Password);

        if (!validPassword)
            return BadRequest("Contraseña actual incorrecta");

        user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

        await _context.SaveChangesAsync();

        return Ok("Contraseña actualizada correctamente");
    }

    // =========================
    // PERFIL
    // =========================
    [Authorize]
    [HttpGet("Profile")]
    public IActionResult Profile()
    {
        var username = User.Identity?.Name;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var tenantId = User.FindFirst("TenantId")?.Value;

        return Ok(new
        {
            username,
            role,
            tenantId
        });
    }
}

// =========================
// MODELOS
// =========================

public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
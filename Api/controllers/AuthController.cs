using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Api.Security;
using Api.Domain.Entities;
using System.Security.Claims;
using Api.Controllers.Models;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;

    public AuthController(JwtService jwtService)
    {
        _jwtService = jwtService;
    }

    private static List<User> users = new List<User>
    {
        new User
        {
            Id = 1,
            Username = "admin",
            Password = "123",
            Role = "Admin",
            TenantId = "Tenant1"
        },
        new User
        {
            Id = 2,
            Username = "user",
            Password = "123",
            Role = "User",
            TenantId = "Tenant2"
        }
    };

    [HttpPost("Login")]
    public IActionResult Login(LoginRequest request)
    {
        var user = users.FirstOrDefault(u =>
            u.Username == request.Username &&
            u.Password == request.Password
        );

        if (user == null)
            return Unauthorized("Credenciales incorrectas");

        var token = _jwtService.GenerateToken(user);

        return Ok(new { token });
    }

    [HttpPost("CambioDeClave")]
    [Authorize]
    public IActionResult CambioDeClave([FromBody] CambioClaveRequest request)
    {
        var username = User.Identity.Name;

        var user = users.FirstOrDefault(u => u.Username == username);

        if (user == null)
            return NotFound("Usuario no encontrado");

        if (user.Password != request.PasswordActual)
            return BadRequest("Contraseña actual incorrecta");

        user.Password = request.PasswordNueva;

        return Ok(new
        {
            message = "Contraseña actualizada correctamente"
        });
    }

    [HttpPost("OlvideMiClave")]
    public IActionResult OlvideMiClave(ForgotPasswordRequest request)
    {
        Console.WriteLine($"Solicitud de recuperación para usuario: {request.Username}");

        return Ok("Solicitud registrada en logs");
    }

    [Authorize]
    [HttpGet("Profile")]
    public IActionResult Profile()
    {
        var username = User.Identity.Name;
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

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}

public class ForgotPasswordRequest
{
    public string Username { get; set; }
}
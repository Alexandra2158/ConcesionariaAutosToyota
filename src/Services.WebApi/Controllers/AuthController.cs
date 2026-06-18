using ConcesionariaAutosToyota.Trade.Application.UseCases.Commands.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcesionariaAutosToyota.Trade.Services.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly LoginUserHandler _loginHandler;
    private readonly ILogger<AuthController> _logger;

    public AuthController(LoginUserHandler loginHandler, ILogger<AuthController> logger)
    {
        _loginHandler = loginHandler;
        _logger = logger;
    }

    /// <summary>Login: retorna JWT token con roles y claims.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginUserResult), 200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var result = await _loginHandler.HandleAsync(command);
        if (!result.Success)
        {
            _logger.LogWarning("Login fallido para usuario: {Username}", command.Username);
            return Unauthorized(new { message = result.Mensaje });
        }
        _logger.LogInformation("Login exitoso para: {Username} [{Rol}]", command.Username, result.Rol);
        return Ok(result);
    }

    /// <summary>Endpoint protegido: solo accesible con JWT válido.</summary>
    [HttpGet("perfil")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public IActionResult GetPerfil()
    {
        var username = User.Identity?.Name;
        var rol = User.Claims.FirstOrDefault(c => c.Type.Contains("role"))?.Value;
        return Ok(new { usuario = username, rol, mensaje = "Token válido ✅" });
    }

    /// <summary>Endpoint protegido solo para Admins.</summary>
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200)]
    [ProducesResponseType(403)]
    public IActionResult GetAdmin()
        => Ok(new { mensaje = "Bienvenido Admin. Acceso autorizado 🔐" });
}

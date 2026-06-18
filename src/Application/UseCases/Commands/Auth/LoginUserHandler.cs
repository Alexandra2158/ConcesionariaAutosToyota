using ConcesionariaAutosToyota.Trade.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ConcesionariaAutosToyota.Trade.Application.UseCases.Commands.Auth;

public class LoginUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public LoginUserHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginUserResult> HandleAsync(LoginUserCommand command)
    {
        var user = await _userRepository.GetByUsernameAsync(command.Username);
        if (user == null || !user.Activo)
            return new LoginUserResult { Success = false, Mensaje = "Usuario no encontrado o inactivo." };

        var hash = ComputeHash(command.Password);
        if (user.PasswordHash != hash)
            return new LoginUserResult { Success = false, Mensaje = "Credenciales inválidas." };

        var token = _tokenService.GenerateToken(user);
        return new LoginUserResult { Success = true, Token = token, Rol = user.Rol, Mensaje = "Login exitoso." };
    }

    private static string ComputeHash(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
}

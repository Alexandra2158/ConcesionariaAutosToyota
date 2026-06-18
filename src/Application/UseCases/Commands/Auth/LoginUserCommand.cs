namespace ConcesionariaAutosToyota.Trade.Application.UseCases.Commands.Auth;

public class LoginUserCommand
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginUserResult
{
    public bool Success { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
}

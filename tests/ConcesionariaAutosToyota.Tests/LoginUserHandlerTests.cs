using ConcesionariaAutosToyota.Trade.Application.Interfaces;
using ConcesionariaAutosToyota.Trade.Application.UseCases.Commands.Auth;
using ConcesionariaAutosToyota.Trade.Domain.Entities;
using Moq;
using Xunit;

namespace ConcesionariaAutosToyota.Tests;

public class LoginUserHandlerTests
{
    // SHA256("Admin1234!")
    private const string AdminPasswordHash = "XOQa2mTx6P+wrPqvpiKxQUOPOld3eF5/C4MPtz5A09Y=";

    // ── Test 4: Login exitoso retorna token ──────────────────────────────────
    [Fact]
    public async Task LoginHandler_ConCredencialesValidas_RetornaExito()
    {
        // Arrange
        var mockUserRepo = new Mock<IUserRepository>();
        var mockTokenSvc = new Mock<ITokenService>();

        var user = new User { Id = 1, Username = "admin", PasswordHash = AdminPasswordHash,
                              Rol = "Admin", Activo = true };

        mockUserRepo.Setup(r => r.GetByUsernameAsync("admin")).ReturnsAsync(user);
        mockTokenSvc.Setup(t => t.GenerateToken(user)).Returns("jwt.token.mock");

        var handler = new LoginUserHandler(mockUserRepo.Object, mockTokenSvc.Object);
        var command = new LoginUserCommand { Username = "admin", Password = "Admin1234!" };

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("jwt.token.mock", result.Token);
        Assert.Equal("Admin", result.Rol);
        mockTokenSvc.Verify(t => t.GenerateToken(It.IsAny<User>()), Times.Once);
    }

    // ── Test 5: Login fallido con password incorrecto ────────────────────────
    [Fact]
    public async Task LoginHandler_ConPasswordIncorrecto_RetornaFallo()
    {
        // Arrange
        var mockUserRepo = new Mock<IUserRepository>();
        var mockTokenSvc = new Mock<ITokenService>();

        var user = new User { Id = 1, Username = "admin", PasswordHash = AdminPasswordHash,
                              Rol = "Admin", Activo = true };

        mockUserRepo.Setup(r => r.GetByUsernameAsync("admin")).ReturnsAsync(user);

        var handler = new LoginUserHandler(mockUserRepo.Object, mockTokenSvc.Object);
        var command = new LoginUserCommand { Username = "admin", Password = "WrongPassword!" };

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("inválidas", result.Mensaje);
        mockTokenSvc.Verify(t => t.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    // ── Test 6: Login con usuario inexistente retorna fallo ──────────────────
    [Fact]
    public async Task LoginHandler_UsuarioNoExiste_RetornaFallo()
    {
        // Arrange
        var mockUserRepo = new Mock<IUserRepository>();
        var mockTokenSvc = new Mock<ITokenService>();

        mockUserRepo.Setup(r => r.GetByUsernameAsync("noexiste")).ReturnsAsync((User?)null);

        var handler = new LoginUserHandler(mockUserRepo.Object, mockTokenSvc.Object);
        var command = new LoginUserCommand { Username = "noexiste", Password = "1234" };

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.False(result.Success);
        mockTokenSvc.Verify(t => t.GenerateToken(It.IsAny<User>()), Times.Never);
    }
}

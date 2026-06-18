using ConcesionariaAutosToyota.Trade.Domain.Entities;

namespace ConcesionariaAutosToyota.Trade.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}

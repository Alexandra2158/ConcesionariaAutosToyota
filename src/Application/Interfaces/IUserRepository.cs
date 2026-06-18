using ConcesionariaAutosToyota.Trade.Domain.Entities;

namespace ConcesionariaAutosToyota.Trade.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int id);
}

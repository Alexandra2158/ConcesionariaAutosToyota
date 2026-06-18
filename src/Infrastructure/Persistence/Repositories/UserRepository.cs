using ConcesionariaAutosToyota.Trade.Application.Interfaces;
using ConcesionariaAutosToyota.Trade.Domain.Entities;
using ConcesionariaAutosToyota.Trade.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ConcesionariaAutosToyota.Trade.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
        => await _context.Users.AsNoTracking()
                               .FirstOrDefaultAsync(u => u.Username == username && u.Activo);

    public async Task<User?> GetByIdAsync(int id)
        => await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
}

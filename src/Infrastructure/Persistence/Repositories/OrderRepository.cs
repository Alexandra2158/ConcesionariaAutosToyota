using ConcesionariaAutosToyota.Trade.Application.Interfaces;
using ConcesionariaAutosToyota.Trade.Domain.Entities;
using ConcesionariaAutosToyota.Trade.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ConcesionariaAutosToyota.Trade.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
        => await _context.Orders.Include(o => o.Customer).Include(o => o.Stock)
                                .AsNoTracking().ToListAsync();

    public async Task<Order?> GetByIdAsync(int id)
        => await _context.Orders.Include(o => o.Customer).Include(o => o.Stock)
                                .AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);

    public async Task<Order> AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }
}

using ConcesionariaAutosToyota.Trade.Application.Interfaces;
using ConcesionariaAutosToyota.Trade.Domain.Entities;
using ConcesionariaAutosToyota.Trade.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ConcesionariaAutosToyota.Trade.Infrastructure.Persistence.Repositories;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _context;

    public StockRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Stock>> GetAllAsync()
        => await _context.Stocks.AsNoTracking().ToListAsync();

    public async Task<Stock?> GetByIdAsync(int id)
        => await _context.Stocks.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

    public async Task<Stock> AddAsync(Stock stock)
    {
        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();
        return stock;
    }

    public async Task<Stock> UpdateAsync(Stock stock)
    {
        _context.Stocks.Update(stock);
        await _context.SaveChangesAsync();
        return stock;
    }

    public async Task DeleteAsync(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);
        if (stock != null)
        {
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
        }
    }
}

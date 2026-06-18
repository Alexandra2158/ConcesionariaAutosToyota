using ConcesionariaAutosToyota.Trade.Domain.Entities;

namespace ConcesionariaAutosToyota.Trade.Application.Interfaces;

public interface IStockRepository
{
    Task<IEnumerable<Stock>> GetAllAsync();
    Task<Stock?> GetByIdAsync(int id);
    Task<Stock> AddAsync(Stock stock);
    Task<Stock> UpdateAsync(Stock stock);
    Task DeleteAsync(int id);
}

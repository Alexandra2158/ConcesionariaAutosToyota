using ConcesionariaAutosToyota.Trade.Domain.Entities;

namespace ConcesionariaAutosToyota.Trade.Application.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<Order> AddAsync(Order order);
    Task<Order> UpdateAsync(Order order);
}

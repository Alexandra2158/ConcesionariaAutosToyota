using ConcesionariaAutosToyota.Trade.Application.Interfaces;
using OrderEntity = ConcesionariaAutosToyota.Trade.Domain.Entities.Order;

namespace ConcesionariaAutosToyota.Trade.Application.UseCases.Queries.Order;

public class GetOrdersQuery { }

public class GetOrdersHandler
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<OrderEntity>> HandleAsync(GetOrdersQuery query)
        => await _orderRepository.GetAllAsync();
}

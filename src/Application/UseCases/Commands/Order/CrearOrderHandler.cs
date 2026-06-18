using ConcesionariaAutosToyota.Trade.Application.Interfaces;
using ConcesionariaAutosToyota.Trade.Domain.Events;
using OrderEntity = ConcesionariaAutosToyota.Trade.Domain.Entities.Order;

namespace ConcesionariaAutosToyota.Trade.Application.UseCases.Commands.Order;

public class CrearOrderHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IEventPublisher _eventPublisher;

    public CrearOrderHandler(IOrderRepository orderRepository, IStockRepository stockRepository, IEventPublisher eventPublisher)
    {
        _orderRepository = orderRepository;
        _stockRepository = stockRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<OrderEntity> HandleAsync(CrearOrderCommand command)
    {
        var stock = await _stockRepository.GetByIdAsync(command.StockId)
            ?? throw new Exception($"Stock con Id {command.StockId} no encontrado.");

        var order = new OrderEntity
        {
            CustomerId = command.CustomerId,
            StockId = command.StockId,
            Total = stock.Precio,
            Estado = "Pendiente",
            Notas = command.Notas
        };

        var orderCreada = await _orderRepository.AddAsync(order);

        var evento = new OrderCreatedEvent
        {
            OrderId = orderCreada.Id,
            StockId = orderCreada.StockId,
            CustomerId = orderCreada.CustomerId,
            Total = orderCreada.Total
        };
        await _eventPublisher.PublishAsync(evento, "order_created");

        return orderCreada;
    }
}

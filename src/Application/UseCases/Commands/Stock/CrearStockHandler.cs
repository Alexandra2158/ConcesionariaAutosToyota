using ConcesionariaAutosToyota.Trade.Application.Interfaces;
using StockEntity = ConcesionariaAutosToyota.Trade.Domain.Entities.Stock;

namespace ConcesionariaAutosToyota.Trade.Application.UseCases.Commands.Stock;

public class CrearStockHandler
{
    private readonly IStockRepository _stockRepository;

    public CrearStockHandler(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task<StockEntity> HandleAsync(CrearStockCommand command)
    {
        var stock = new StockEntity
        {
            Modelo = command.Modelo,
            Marca = "Toyota",
            Anio = command.Anio,
            Color = command.Color,
            Precio = command.Precio,
            Cantidad = command.Cantidad,
            NumeroVIN = command.NumeroVIN,
            Disponible = true
        };
        return await _stockRepository.AddAsync(stock);
    }
}

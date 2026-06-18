using ConcesionariaAutosToyota.Trade.Application.Interfaces;
using StockEntity = ConcesionariaAutosToyota.Trade.Domain.Entities.Stock;

namespace ConcesionariaAutosToyota.Trade.Application.UseCases.Queries.Stock;

public class GetStocksQuery { }

public class GetStocksHandler
{
    private readonly IStockRepository _stockRepository;

    public GetStocksHandler(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task<IEnumerable<StockEntity>> HandleAsync(GetStocksQuery query)
        => await _stockRepository.GetAllAsync();
}

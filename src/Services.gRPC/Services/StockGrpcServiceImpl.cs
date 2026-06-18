using ConcesionariaAutosToyota.Trade.Application.Interfaces;
using Grpc.Core;

namespace ConcesionariaAutosToyota.Trade.Services.gRPC.Services;

public class StockGrpcServiceImpl : StockGrpcService.StockGrpcServiceBase
{
    private readonly IStockRepository _stockRepository;
    private readonly ILogger<StockGrpcServiceImpl> _logger;

    public StockGrpcServiceImpl(IStockRepository stockRepository, ILogger<StockGrpcServiceImpl> logger)
    {
        _stockRepository = stockRepository;
        _logger = logger;
    }

    public override async Task<GetAllStocksResponse> GetAllStocks(
        GetAllStocksRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC: GetAllStocks invocado.");
        var stocks = await _stockRepository.GetAllAsync();
        var response = new GetAllStocksResponse();

        foreach (var s in stocks)
        {
            response.Stocks.Add(new StockReply
            {
                Id = s.Id,
                Modelo = s.Modelo,
                Marca = s.Marca,
                Anio = s.Anio,
                Color = s.Color,
                Precio = (double)s.Precio,
                Cantidad = s.Cantidad,
                NumeroVin = s.NumeroVIN,
                Disponible = s.Disponible
            });
        }
        return response;
    }

    public override async Task<StockReply> GetStockById(
        GetStockByIdRequest request, ServerCallContext context)
    {
        var stock = await _stockRepository.GetByIdAsync(request.Id);
        if (stock == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Stock {request.Id} no encontrado."));

        return new StockReply
        {
            Id = stock.Id,
            Modelo = stock.Modelo,
            Marca = stock.Marca,
            Anio = stock.Anio,
            Color = stock.Color,
            Precio = (double)stock.Precio,
            Cantidad = stock.Cantidad,
            NumeroVin = stock.NumeroVIN,
            Disponible = stock.Disponible
        };
    }
}

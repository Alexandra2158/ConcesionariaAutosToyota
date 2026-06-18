using ConcesionariaAutosToyota.Trade.Application.Interfaces;
using ConcesionariaAutosToyota.Trade.Application.UseCases.Commands.Stock;
using ConcesionariaAutosToyota.Trade.Application.UseCases.Queries.Stock;
using ConcesionariaAutosToyota.Trade.Domain.Entities;
using Moq;
using Xunit;

namespace ConcesionariaAutosToyota.Tests;

public class StockHandlerTests
{
    // ── Test 1: CrearStockHandler debe retornar el stock creado ──────────────
    [Fact]
    public async Task CrearStockHandler_DebeRetornarStockCreado()
    {
        // Arrange
        var mockRepo = new Mock<IStockRepository>();
        var command = new CrearStockCommand
        {
            Modelo = "Corolla",
            Anio = 2026,
            Color = "Blanco",
            Precio = 25000m,
            Cantidad = 5,
            NumeroVIN = "1HGBH41JXMN109186"
        };

        var stockEsperado = new Stock
        {
            Id = 1,
            Modelo = "Corolla",
            Anio = 2026,
            Color = "Blanco",
            Precio = 25000m,
            Cantidad = 5,
            NumeroVIN = "1HGBH41JXMN109186",
            Disponible = true
        };

        mockRepo.Setup(r => r.AddAsync(It.IsAny<Stock>()))
                .ReturnsAsync(stockEsperado);

        var handler = new CrearStockHandler(mockRepo.Object);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Corolla", result.Modelo);
        Assert.Equal(25000m, result.Precio);
        Assert.True(result.Disponible);
        mockRepo.Verify(r => r.AddAsync(It.IsAny<Stock>()), Times.Once);
    }

    // ── Test 2: GetStocksHandler debe retornar lista de stocks ───────────────
    [Fact]
    public async Task GetStocksHandler_DebeRetornarListaDeStocks()
    {
        // Arrange
        var mockRepo = new Mock<IStockRepository>();
        var stocksEsperados = new List<Stock>
        {
            new() { Id = 1, Modelo = "Corolla",  Precio = 25000m, Disponible = true },
            new() { Id = 2, Modelo = "Hilux",    Precio = 45000m, Disponible = true },
            new() { Id = 3, Modelo = "Yaris",    Precio = 18000m, Disponible = false }
        };

        mockRepo.Setup(r => r.GetAllAsync())
                .ReturnsAsync(stocksEsperados);

        var handler = new GetStocksHandler(mockRepo.Object);

        // Act
        var result = (await handler.HandleAsync(new GetStocksQuery())).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(result, s => s.Modelo == "Hilux");
        mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
    }

    // ── Test 3: CrearStockHandler llama AddAsync con datos correctos ─────────
    [Fact]
    public async Task CrearStockHandler_LlamaAddAsyncConDatosCorrectos()
    {
        // Arrange
        var mockRepo = new Mock<IStockRepository>();
        Stock? stockCapturado = null;

        mockRepo.Setup(r => r.AddAsync(It.IsAny<Stock>()))
                .Callback<Stock>(s => stockCapturado = s)
                .ReturnsAsync((Stock s) => s);

        var handler = new CrearStockHandler(mockRepo.Object);
        var command = new CrearStockCommand
        {
            Modelo = "RAV4",
            Anio = 2025,
            Color = "Rojo",
            Precio = 38000m,
            Cantidad = 2,
            NumeroVIN = "2T1BURHE0JC034785"
        };

        // Act
        await handler.HandleAsync(command);

        // Assert
        Assert.NotNull(stockCapturado);
        Assert.Equal("RAV4", stockCapturado!.Modelo);
        Assert.Equal("Toyota", stockCapturado.Marca); // Marca siempre es Toyota
        Assert.True(stockCapturado.Disponible);
    }
}

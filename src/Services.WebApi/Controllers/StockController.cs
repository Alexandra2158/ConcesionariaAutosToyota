using ConcesionariaAutosToyota.Trade.Application.UseCases.Commands.Stock;
using ConcesionariaAutosToyota.Trade.Application.UseCases.Queries.Stock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcesionariaAutosToyota.Trade.Services.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class StockController : ControllerBase
{
    private readonly GetStocksHandler _getStocksHandler;
    private readonly CrearStockHandler _crearStockHandler;
    private readonly ILogger<StockController> _logger;

    public StockController(GetStocksHandler getStocksHandler,
        CrearStockHandler crearStockHandler,
        ILogger<StockController> logger)
    {
        _getStocksHandler = getStocksHandler;
        _crearStockHandler = crearStockHandler;
        _logger = logger;
    }

    /// <summary>Obtiene todo el inventario de vehículos Toyota.</summary>
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await _getStocksHandler.HandleAsync(new GetStocksQuery());
        return Ok(stocks);
    }

    /// <summary>Agrega un nuevo vehículo al inventario (Solo Admin).</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CrearStockCommand command)
    {
        var stock = await _crearStockHandler.HandleAsync(command);
        _logger.LogInformation("Stock creado: {Modelo} VIN:{VIN}", stock.Modelo, stock.NumeroVIN);
        return CreatedAtAction(nameof(GetAll), new { id = stock.Id }, stock);
    }
}

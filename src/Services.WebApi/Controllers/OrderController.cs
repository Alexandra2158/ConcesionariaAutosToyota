using ConcesionariaAutosToyota.Trade.Application.UseCases.Commands.Order;
using ConcesionariaAutosToyota.Trade.Application.UseCases.Queries.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcesionariaAutosToyota.Trade.Services.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly GetOrdersHandler _getOrdersHandler;
    private readonly CrearOrderHandler _crearOrderHandler;
    private readonly ILogger<OrderController> _logger;

    public OrderController(GetOrdersHandler getOrdersHandler,
        CrearOrderHandler crearOrderHandler,
        ILogger<OrderController> logger)
    {
        _getOrdersHandler = getOrdersHandler;
        _crearOrderHandler = crearOrderHandler;
        _logger = logger;
    }

    /// <summary>Lista todas las órdenes de compra.</summary>
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _getOrdersHandler.HandleAsync(new GetOrdersQuery());
        return Ok(orders);
    }

    /// <summary>Crea una nueva orden de compra y publica evento OrderCreated.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Vendedor")]
    [ProducesResponseType(201)]
    public async Task<IActionResult> Create([FromBody] CrearOrderCommand command)
    {
        var order = await _crearOrderHandler.HandleAsync(command);
        _logger.LogInformation("Order #{Id} creada, evento publicado.", order.Id);
        return CreatedAtAction(nameof(GetAll), new { id = order.Id }, order);
    }
}

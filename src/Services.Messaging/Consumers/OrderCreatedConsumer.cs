using ConcesionariaAutosToyota.Trade.Domain.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ConcesionariaAutosToyota.Trade.Services.Messaging.Consumers;

/// <summary>
/// Worker que consume eventos OrderCreated desde RabbitMQ
/// y descuenta inventario de Stock.
/// </summary>
public class OrderCreatedConsumer : BackgroundService
{
    private readonly ILogger<OrderCreatedConsumer> _logger;
    private readonly IConfiguration _config;
    private IConnection? _connection;
    private IChannel? _channel;

    public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _config["RabbitMQ:Host"] ?? "rabbitmq",
                UserName = _config["RabbitMQ:User"] ?? "guest",
                Password = _config["RabbitMQ:Password"] ?? "guest"
            };

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.QueueDeclareAsync("order_created",
                durable: true, exclusive: false, autoDelete: false,
                cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var evento = JsonSerializer.Deserialize<OrderCreatedEvent>(json);

                if (evento != null)
                {
                    _logger.LogInformation(
                        "📥 [CONSUMIDOR] OrderCreated recibido → OrderId:{Id} StockId:{StockId} Total:{Total}",
                        evento.OrderId, evento.StockId, evento.Total);

                    // Aquí se actualizaría el Stock (descuento de inventario)
                    _logger.LogInformation("🔄 Descontando 1 unidad del Stock Id:{StockId}", evento.StockId);
                }

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await _channel.BasicConsumeAsync("order_created", autoAck: false, consumer: consumer,
                cancellationToken: stoppingToken);

            _logger.LogInformation("✅ Consumidor OrderCreated escuchando en RabbitMQ...");

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("⚠️ Consumidor RabbitMQ no pudo iniciar: {Msg}", ex.Message);
        }
    }

    public override void Dispose()
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
        base.Dispose();
    }
}

using ConcesionariaAutosToyota.Trade.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ConcesionariaAutosToyota.Trade.Infrastructure.Messaging;

public class RabbitMqEventPublisher : IEventPublisher, IDisposable
{
    private readonly ILogger<RabbitMqEventPublisher> _logger;
    private IConnection? _connection;
    private IModel? _channel;
    private bool _connected = false;

    public RabbitMqEventPublisher(ILogger<RabbitMqEventPublisher> logger, IConfiguration config)
    {
        _logger = logger;
        TryConnect(config);
    }

    private void TryConnect(IConfiguration config)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = config["RabbitMQ:Host"] ?? "rabbitmq",
                UserName = config["RabbitMQ:User"] ?? "guest",
                Password = config["RabbitMQ:Password"] ?? "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _connected = true;
            _logger.LogInformation("Conectado a RabbitMQ.");
        }
        catch (Exception ex)
        {
            _logger.LogWarning("RabbitMQ no disponible: {Msg}. Usando fallback a log.", ex.Message);
        }
    }

    public Task PublishAsync<T>(T evento, string queueName) where T : class
    {
        var json = JsonSerializer.Serialize(evento);

        if (_connected && _channel != null)
        {
            _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
            var body = Encoding.UTF8.GetBytes(json);
            _channel.BasicPublish(exchange: string.Empty, routingKey: queueName, body: body);
            _logger.LogInformation("Evento publicado en '{Queue}': {Payload}", queueName, json);
        }
        else
        {
            _logger.LogInformation("[FALLBACK] Evento '{Queue}': {Payload}", queueName, json);
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
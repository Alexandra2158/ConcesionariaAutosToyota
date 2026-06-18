namespace ConcesionariaAutosToyota.Trade.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(T evento, string queueName) where T : class;
}

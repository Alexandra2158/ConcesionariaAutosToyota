namespace ConcesionariaAutosToyota.Trade.Domain.Events;

public class OrderCreatedEvent
{
    public int OrderId { get; set; }
    public int StockId { get; set; }
    public int CustomerId { get; set; }
    public decimal Total { get; set; }
    public DateTime OcurridoEn { get; set; } = DateTime.UtcNow;
    public string TipoEvento => "OrderCreated";
}

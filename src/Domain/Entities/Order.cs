namespace ConcesionariaAutosToyota.Trade.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int StockId { get; set; }
    public DateTime FechaOrden { get; set; } = DateTime.UtcNow;
    public decimal Total { get; set; }
    public string Estado { get; set; } = "Pendiente"; // Pendiente, Confirmada, Cancelada
    public string Notas { get; set; } = string.Empty;

    public Customer? Customer { get; set; }
    public Stock? Stock { get; set; }
}

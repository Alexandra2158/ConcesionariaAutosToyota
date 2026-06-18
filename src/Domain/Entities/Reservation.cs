namespace ConcesionariaAutosToyota.Trade.Domain.Entities;

public class Reservation
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int StockId { get; set; }
    public DateTime FechaReserva { get; set; } = DateTime.UtcNow;
    public DateTime FechaExpiracion { get; set; }
    public string Estado { get; set; } = "Activa"; // Activa, Expirada, Convertida
    public decimal MontoReserva { get; set; }

    public Customer? Customer { get; set; }
    public Stock? Stock { get; set; }
}

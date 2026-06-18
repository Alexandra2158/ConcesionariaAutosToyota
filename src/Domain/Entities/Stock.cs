namespace ConcesionariaAutosToyota.Trade.Domain.Entities;

public class Stock
{
    public int Id { get; set; }
    public string Modelo { get; set; } = string.Empty;
    public string Marca { get; set; } = "Toyota";
    public int Anio { get; set; }
    public string Color { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Cantidad { get; set; }
    public string NumeroVIN { get; set; } = string.Empty;
    public bool Disponible { get; set; } = true;
    public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;
}

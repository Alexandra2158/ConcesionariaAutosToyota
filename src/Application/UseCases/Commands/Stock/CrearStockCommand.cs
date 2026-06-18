namespace ConcesionariaAutosToyota.Trade.Application.UseCases.Commands.Stock;

public class CrearStockCommand
{
    public string Modelo { get; set; } = string.Empty;
    public int Anio { get; set; }
    public string Color { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Cantidad { get; set; }
    public string NumeroVIN { get; set; } = string.Empty;
}

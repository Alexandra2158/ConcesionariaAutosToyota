namespace ConcesionariaAutosToyota.Trade.Application.UseCases.Commands.Order;

public class CrearOrderCommand
{
    public int CustomerId { get; set; }
    public int StockId { get; set; }
    public string Notas { get; set; } = string.Empty;
}

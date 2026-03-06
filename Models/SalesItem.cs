namespace SalesApi.Models;

public class SalesItem
{
    public int Id { get; private set; }

    public int SalesId { get; private set; }
    public Sales Sales { get; private set; }

    public int ProductId { get; private set; }
    public Product Product { get; private set; }

    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
}

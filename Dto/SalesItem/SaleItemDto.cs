
namespace SalesApi.Dto.Sales;

public class SaleItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal {  get; set; }
}

public class SalesDto
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<SaleItemDto> Items { get; set; } = new();
}
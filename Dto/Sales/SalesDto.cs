using SalesApi.Dto.SalesItem;

namespace SalesApi.Dto.Sales;

public class SalesDto
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<SaleItemDto> Items { get; set; } = new();
}

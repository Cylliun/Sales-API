namespace SalesApi.Dto.SalesItem;

public class CreateSaleItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class CreateSalesDto
{
    public List<CreateSalesDto> Items { get; set; } = new();
}

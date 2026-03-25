using SalesApi.Dto.SalesItem;

namespace SalesApi.Dto.Sales;

public class CreateSalesDto
{
   public List<CreateSaleItemDto> Items { get; set; } = new List<CreateSaleItemDto>();
}

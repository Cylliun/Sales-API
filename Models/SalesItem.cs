public class SalesItem
{
    public int Id { get; private set; }

    public int ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    public Sale Sale { get; private set; }

    private SalesItem() { }

    public SalesItem(int productId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero");

        if (unitPrice <= 0)
            throw new ArgumentException("Unit price must be greater than zero");

        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public void SetSale(Sale sale)
    {
        Sale = sale;
    }

    public decimal CalculateSubTotal()
    {
        return Quantity * UnitPrice;
    }
}
using SalesApi.Models;

public class Sale
{
    public int Id { get; private set; }

    public decimal TotalAmount { get; private set; }
    public bool IsCanceled { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public List<SalesItem> Items { get; private set; }

    public int UserId { get; private set; }
    public User User { get; private set; }

    private Sale() { }

    public Sale(int userId)
    {
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        Items = new List<SalesItem>();
    }

    public void AddItem(SalesItem item)
    {
        item.SetSale(this);
        Items.Add(item);
        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        TotalAmount = Items.Sum(i => i.CalculateSubTotal());
    }

    public void Cancel()
    {
        if (IsCanceled)
            throw new InvalidOperationException("Sale already canceled");

        IsCanceled = true;
    }
}
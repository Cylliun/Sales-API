namespace SalesApi.Models;

public class Sales
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public User User { get; private set; }

    public decimal TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public List<SalesItem> Items { get; private set; } = new List<SalesItem>();
}

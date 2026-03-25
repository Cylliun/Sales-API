namespace SalesApi.Models;

public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }

    public int UserId { get; private set; }
    public User User { get; private set; }


    private Product() { } // necessário para o Entity Framework

    public Product(string name, decimal price, int stock, int userId)
    {
        Validate(name, price, stock);
        Name = name;
        Price = price;
        Stock = stock;
        UserId = userId;
         
    }

    public void Update(string name, decimal price)
    {
        Validate(name, price, Stock);
        Name = name;
        Price = price;
    }

    private void Validate(string name, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome não pode ser vazio");

        if (price <= 0)
            throw new ArgumentException("Preço não pode ser 0 ou inferior");

        if (stock < 0)
            throw new ArgumentException("Estoque não pode ser negativo");
    }

    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero");

        if (Stock < quantity)
            throw new InvalidOperationException("Estoque insuficiente");

        Stock -= quantity;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero");

        Stock += quantity;
    }
}
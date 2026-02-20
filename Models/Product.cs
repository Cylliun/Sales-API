namespace SalesApi.Models;

public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }

    public Product(string name, decimal price)
    {
        Validate(name, price);
        Name = name;
        Price = price;
    }

    public void Update(String name, decimal price)
    {
        Validate(name, price);
        Name = name;
        Price = price;
    }

    public void Validate(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome não pode ser vazio");
        if (price <= 0)
            throw new ArgumentException("Preço não pode ser 0 ou inferior");
    }
}

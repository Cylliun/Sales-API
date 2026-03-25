using Microsoft.EntityFrameworkCore;
using SalesApi.Models;

namespace SalesApi.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SalesItem> SalesItems { get; set; }
}

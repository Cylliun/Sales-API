using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SalesApi.Data;

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

        optionsBuilder.UseSqlServer("Server=localhost;Database=SalesDb;Trusted_Connection=True;TrustServerCertificate=True");

        return new DataContext(optionsBuilder.Options);

    }
}

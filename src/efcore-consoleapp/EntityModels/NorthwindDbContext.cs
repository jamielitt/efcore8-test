using Microsoft.EntityFrameworkCore;

namespace efcore_consoleapp.EntityModels;

public class NorthwindDb : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        string databaseName = "Northwind.db";
        string path = Path.Combine(Environment.CurrentDirectory, databaseName);
        string connectionString = $"Data Source = {path}";
        optionsBuilder.UseSqlite(connectionString);
    }
}
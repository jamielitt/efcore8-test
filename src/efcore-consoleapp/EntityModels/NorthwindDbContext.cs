using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Northwind.EntityModels;

namespace efcore_consoleapp.EntityModels;

public class NorthwindDb : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Play nicely with the base class
        base.OnConfiguring(optionsBuilder);

        string databaseName = "Northwind.db";
        string path = Path.Combine(Environment.CurrentDirectory, databaseName);
        string connectionString = $"Data Source = {path}";
        optionsBuilder.UseSqlite(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Example of usign Fluent API instead of attributes
        modelBuilder.Entity<Category>()
            .Property(e => e.CategoryName)
            .IsRequired()
            .HasMaxLength(15);

        // Some Sqlite specific configuration.
        if (Database.ProviderName?.Contains("Sqlite") ?? false)
        {
            modelBuilder.Entity<Product>()
                .Property(e => e.ProductName)
                .HasConversion<double>();
        }
    }
}
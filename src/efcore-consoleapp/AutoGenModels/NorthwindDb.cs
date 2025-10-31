using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace efcore_consoleapp.AutoGen;

public partial class NorthwindDb : DbContext
{
    public NorthwindDb()
    {
    }

    public NorthwindDb(DbContextOptions<NorthwindDb> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    {
        optionsBuilder.UseSqlite("Data Source=Northwind.db");

        // This can be modified by adding in the event to filter on:   
        optionsBuilder.LogTo(WriteLine, new[] { RelationalEventId.CommandExecuting })
#if DEBUG
            .EnableSensitiveDataLogging() // Include SQL parameters only in debug
            .EnableDetailedErrors()
#endif
        ;

        // Enables Lazy Loading - "but" - be careful with this as for every referenced entity
        // where you actually want someting, it will do a round trip to the database.
        // .Include() gets around this.. See query for example.
        optionsBuilder.UseLazyLoadingProxies();
}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CategoryId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ProductId).ValueGeneratedNever();
            entity.Property(e => e.ReorderLevel).HasDefaultValue((short)0);
            entity.Property(e => e.UnitPrice).HasDefaultValue(0.0);
            entity.Property(e => e.UnitsInStock).HasDefaultValue((short)0);
            entity.Property(e => e.UnitsOnOrder).HasDefaultValue((short)0);
        });

        // A global filter to remove discontinued products. This will be applied to all queries run
        modelBuilder.Entity<Product>()
            .HasQueryFilter(p => !p.Discontinued);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

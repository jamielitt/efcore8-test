using efcore_consoleapp.AutoGen;
using Microsoft.EntityFrameworkCore; // To use ExecuteUpdate, ExecuteDelete.
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Update;

partial class Program()
{
    private static void ListProducts(int[]? productIdsToHighlight)
    {
        using NorthwindDb db = new();

        if (db.Products is null || !db.Products.Any())
        {
            Fail("No products found");
            return;
        }
        
        WriteLine("| {0,-3} | {1,-35} | {2,8} | {3,5} | {4} |",
            "Id", "Product Name", "Cost", "Stock", "Disc.");

        IQueryable<Product> products = db.Products.Where(p => p.CategoryId == 6);
        var currentColour = ForegroundColor;
        foreach (var p in products)
        {
            bool highlightProduct = productIdsToHighlight.Contains(p.ProductId);
            ForegroundColor = highlightProduct ? ConsoleColor.Green : currentColour;
            
            WriteLine("| {0:000} | {1,-35} | {2,8:$#,##0.00} | {3,5} | {4} |",
                p.ProductId, p.ProductName, p.UnitPrice, p.UnitsInStock, p.Discontinued);
            
            ForegroundColor = currentColour;
        }
    }

    private static (int affected, int productId) AddProduct(int categoryId, string productName, double unitPrice, short stock)
    {
        using NorthwindDb db = new();

        if (db.Products is null)
        {
            return (0, 0);
        }

        Product p = new()
        {
            CategoryId = categoryId,
            ProductName = productName,
            UnitPrice = unitPrice,
            UnitsInStock = stock
        };
        
        EntityEntry<Product> entry = db.Products.Add(p);
        WriteLine("State: {entry.State}, ProductId: {entry.ProductId}");
        int affected = db.SaveChanges();
        WriteLine("State: {entry.State}, ProductId: {entry.ProductId}");
        return (affected, productId: p.ProductId);
    }

    private static (int affected, int productId) IncreaseProductPrice(string productNameStartsWith, double amount)
    {
        using NorthwindDb db = new();

        SectionTitle("Increasing product price example");
        if (db.Products is null)
        {
            WriteLine("No products in database");
            return (0, 0);
        }

        Product product = db.Products
            .First(p => p.ProductName
                .StartsWith(productNameStartsWith));
        
        product.UnitPrice += amount;
        
        int productsAffected = db.SaveChanges();

        if (productsAffected == 0)
        {
            WriteLine("No products affected");
            return (0, 0);
        }
        return  (productsAffected, productId: product.ProductId);
    }
}
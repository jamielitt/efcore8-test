using efcore_consoleapp.AutoGen;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query; // To use Include method.

// This is so we can use the ChangeTracker to control Lazy Loading
using Microsoft.EntityFrameworkCore.ChangeTracking;

partial class Program
{
    private static void QueryingCategories()
    {
        using NorthwindDb db = new();
        SectionTitle("Categories and how many products they have");
      
        // A query to get all categories and their related products.
        IQueryable<Category>? categories;
        
        // Disables the Lazy Loading so we can make a choice on what to do
        db.ChangeTracker.LazyLoadingEnabled = false;
        
        // Eager Loading example (loads everything in one query)
        categories = db.Categories?.Include(c => c.Products);

        // Default if no eager loading
        //categories = db.Categories;
        
        if (categories is null || !categories.Any())
        {
            Fail("No categories found.");
            return;
        }

        // Execute query and enumerate results.
        foreach (Category c in categories)
        {
            // This is how we do explicit loading, we need this if
            // lazy loading and eager loading is not present
            // Comment out this section if Lazy or Eager loading is present
            //CollectionEntry<Category, Product> products =
            //    db.Entry(c).Collection(c2 => c2.Products);
            //if (!products.IsLoaded) products.Load();
            
            WriteLine($"{c.CategoryName} has {c.Products.Count} products.");
        }
    }

    private static void FilterIncludes()
    {
        using NorthwindDb db = new();
        SectionTitle("Products with a minimum number of units in stock");

        string? input;
        int stock;

        do
        {
            Write("Enter a minimum for units in stock: ");
            input = ReadLine();
        }
        while (!int.TryParse(input, out stock));
        
        IQueryable<Category>? categories = db.Categories?
            .Include(c => 
                c.Products.Where(p => p.UnitsInStock >= stock));
        
        if (categories is null || !categories.Any())
        {
            Fail("No categories found.");
            return;
        }
        
        // Dumps out the SQL so we can inspect it
        Info($"ToQueryString: {categories.ToQueryString()}");
        
        foreach (Category c in categories)
        {
            WriteLine("{0} has {1} products with a minimum {2} units in stock.", arg0: c.CategoryName,
                arg1: c.Products.Count, arg2: stock);
            foreach (Product p in c.Products)
            {
                WriteLine($" {p.ProductName} has {p.UnitsInStock} units in stock.");
            }
        }
    }

    private static void QueryingProducts()
    {
        using NorthwindDb db = new();
        
        // This will disable tracking for the context
        //db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        //db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
        
        SectionTitle("Products that cost more than a price, highest at top");
        string? input;
        double price;
        do
        {
            Write("Enter a product price: ");
            input = ReadLine();
        } while (!double.TryParse(input, out price));
        
        IQueryable<Product>? products = db.Products?
            // .AsNoTracking ensures we get a fresh copy from the database (and not return the version
            // that is already in context)
            //.AsNoTracking()
            //.AsNoTrackingWithIdentityResolution()
            .Where(p => p.UnitPrice > price)
            .OrderBy(p => p.UnitPrice)
            .TagWith($"Products with a unit price greater than {price} ordered by the unit price");

        if (products is null || !products.Any())
        {
            Fail("No products found.");
            return;
        }

        Info($"ToQueryString: {products.ToQueryString()}");
        
        foreach (Product p in products)
        {
            WriteLine(
                "{0}: {1} costs {2:$#,##0.00} and has {3} in stock.",
                p.ProductId, p.ProductName, p.UnitPrice, p.UnitsInStock);
        }
    }

    private static void GettingOneProduct()
    {
        using NorthwindDb db = new();
        string? input;
        int productId;
        SectionTitle("Getting a single product");

        do
        {
            Write("Enter a product id: ");
            input =  ReadLine();
        } while (!int.TryParse(input, out productId));
        
        // It doesn't matter how many records are returned with this match, only the first one
        // will be returned
        Product? product = db.Products?.Where(p => p.ProductId == productId).First();
        
        Info($"First: {product?.ProductName}");
        if (product is null) Fail("No product found using First.");
        
        // Single will try and get 2 entities by using LIMIT = 2, if 2 are returned an exception is thrown.
        product = db.Products?.Where(p => p.ProductId == productId).Single();
        
        Info($"Single: {product?.ProductName}");

        if (product is null) Fail("No product found using Single.");
    }

    private static void QueryingWithLike()
    {
        using NorthwindDb db = new();
        SectionTitle("Querying with a like example");
        Write("Enter part of a product name: ");
        string input = ReadLine();
        
        // Is this analagous to the SQL LIKE command?
        IQueryable<Product>? products = db.Products.Where(p => EF.Functions.Like(p.ProductName, $"%{input}%"));
        if (products is null || !products.Any())
        {
            Info("No products found.");
            return;
        }

        foreach (Product p in products)
        {
            WriteLine($"Name: {p.ProductName}, Stock: {p.UnitsInStock}, Discontinued: {p.Discontinued}");
        }
    }

    private static void LazyLoadingWithNoTracking()
    {
        using NorthwindDb db = new();
        SectionTitle("Lazy Loading with no-tracking");
        IQueryable<Product>? products = db.Products.AsNoTracking();

        if (products is null || !products.Any())
        {
            Fail("No products found.");
        }

        foreach (Product p in products)
        {
            WriteLine("{0} is in Category named {1}", p.ProductName, p.Category.CategoryName);
        }
    }
}
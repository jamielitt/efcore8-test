// See https://aka.ms/new-console-template for more information
using efcore_consoleapp.EntityModels;

// Declare new instance of the DB Context
using NorthwindDb db =  new NorthwindDb();

Console.WriteLine("Hello from EF Core 8 Example App!");
Console.WriteLine($"Database Provider: {db.Database.ProviderName}");

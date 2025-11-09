// See https://aka.ms/new-console-template for more information
using efcore_consoleapp.EntityModels;

ConfigureConsole();

// Different types of queries
// QueryingCategories();
// FilterIncludes();
// QueryingProducts();
// GettingOneProduct();
// QueryingWithLike();
// LazyLoadingWithNoTracking();

// var resultAdd = AddProduct(categoryId: 6,
//     productName: "Bob's Big Burgers", unitPrice: 500, stock: 72);
// if (resultAdd.affected == 1)
// {
//     WriteLine($"Add product successful with ID: {resultAdd.productId}.");
// }
// ListProducts(productIdsToHighlight: new[] { resultAdd.productId });

string startsWith = "Bob";
var productsRemoved = DeleteProducts(productNameStartsWith: startsWith);
WriteLine($"{productsRemoved} products have been removed from the database that start with {startsWith}");
# efcore8-test

## Generating the entities

Run this command from inside the main project file


`dotnet ef dbcontext scaffold "Data Source=Northwind.db" Microsoft.EntityFrameworkCore.Sqlite --table Categories --table Products --output-dir AutoGenModels --namespace efcore_consoleapp.AutoGen --data-annotations --context NorthwindDb`
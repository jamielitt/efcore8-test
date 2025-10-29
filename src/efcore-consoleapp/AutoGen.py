import subprocess

DataSource = "Northwind.db"
Frameworks = ["Microsoft.EntityFrameworkCore.Sqlite"]
Tables = ["Categories", "Products"]
OutputDirectory = "AutoGenModels"
Namespace = "efcore_consoleapp.AutoGen"
Context = "NorthwindDb"

def get_input_default_fallback(field, default):
    print(f"Please enter the {field}: ")
    print(f"(Default is {default})")
    user_input = input(">")
    
    if not user_input:
        print(f"{field} kept at {default}")
        user_input = default
    else:
        print(f"{field} set to {user_input}")
    return user_input

def get_frameworks(frameworks):
    print("Please specify the Framework to use:")
    for index, framework in enumerate(frameworks):
        print(f"{index} - {framework}")
    index = input(">")
    return int(index)

def get_tables(tables):
    print("Please specify which tables to get (comma separated):")
    for index, table in enumerate(tables):
        print(f"{table}, ", end="")
    print("")
    tables_string = input(">")
    if not tables_string:
        return tables
    else:
        tables = tables_string.split(",")
    return tables

def generate_command(datasource, framework_index, tables, output_directory, namespace, context):
    command = f"dotnet ef dbcontext scaffold \"Data Source={datasource}\""
    command += f" {Frameworks[framework_index]} "
    
    for table in tables:
        command += f"--table {table} "
       
    command += f"--output-dir {output_directory} "
    command += f"--namespace {namespace} "
    command += "--data-annotations "
    command += f"--context {context}"
    return command
    
print("EF Core Autogen Tool")
print("")
print("Example Command Below:")
print("dotnet ef dbcontext scaffold \"Data Source=Northwind.db\" Microsoft.EntityFrameworkCore.Sqlite --table Categories --table Products --output-dir AutoGenModels --namespace efcore_consoleapp.AutoGen --data-annotations --context NorthwindDb")
print("")

# Get all the needed variables
data_source = get_input_default_fallback("Data Source", DataSource)
index = get_frameworks(Frameworks)
tables = get_tables(Tables)
output_directory = get_input_default_fallback("Output Directory", OutputDirectory)
namespace = get_input_default_fallback("Namespace", Namespace)
context = get_input_default_fallback("Context", Context)

# Generate the command
command_to_run = generate_command(data_source, index, tables, output_directory, namespace, context)
print("Thank you, about to execute the following command:")
print(command_to_run)
print("")

# Running the generated script on the command line
subprocess.run(command_to_run, shell=True)



### 1. Install the required NuGet package:

Ensure that the PostgreSQL Entity Framework Core provider is installed:

```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL -version 8.0.4
dotnet add package Microsoft.EntityFrameworkCore.Tools -version 8.0.4
dotnet add Microsoft.EntityFrameworkCore.Design -version 8.0.4
```

### 2. Install the EF Core tools (if not already installed):

Make sure you have the Entity Framework Core CLI tools installed:

```bash
dotnet tool install --global dotnet-ef
```

### 3. Add a migration:

Once the packages are in place, you can create a migration for your PostgreSQL database with the following command:

```bash
dotnet ef migrations add <MigrationName>
```

For example, to create an "InitialCreate" migration:

```bash
dotnet ef migrations add InitialCreate --project PostgreSqlEF.csproj --startup-project ../../AIChecker
```

### 4. Update the database:

To apply the migration to your PostgreSQL database, use:

```bash
dotnet ef database update --project PostgreSqlEF.csproj --startup-project ../../AIChecker --connection "Host=localhost;Database=AiCheckerDB;Port=5432;User Id=AiChecker;Password=123;Timeout"
```


```bash
dotnet ef migrations list --project PostgreSqlEF.csproj --startup-project ../../AIChecker --connection "Host=localhost;Database=AiCheckerDB;Port=5432;User Id=AiChecker;Password=123"

```
This will execute the migration and create the necessary database schema in PostgreSQL.

If needed set the timeout for the connection string. For example, if you have to create indexes on a large table:


```bash
dotnet ef database update --project PostgreSqlEF.csproj --startup-project ../../AIChecker --connection "Host=localhost;Database=AiCheckerDB;Port=5432;User Id=AiChecker;Password=123;CommandTimeout=600"
```

```bash

## Scaffold the DbContext:

```bash
dotnet ef dbcontext scaffold "Host=localhost;Database=AiCheckerDB;Port=5432;User Id=AiChecker;Password=123" Npgsql.EntityFrameworkCore.PostgreSQL -o DbModels
```

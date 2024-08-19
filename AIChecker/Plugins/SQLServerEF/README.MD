```
Scaffold-DbContext "Server=127.0.0.1;Database=AIChecker;User ID=sa;Password=123456789!_Asdf;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir DbModels
```

This works in a not default directory.
```
dotnet tool install --global dotnet-ef
dotnet ef dbcontext scaffold "Server=127.0.0.1;Database=AIChecker;User ID=sa;Password=123456789!_Asdf;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o DbModels
```
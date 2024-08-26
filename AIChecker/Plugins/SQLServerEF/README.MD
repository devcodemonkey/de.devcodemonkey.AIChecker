```
Scaffold-DbContext "Server=127.0.0.1;Database=AIChecker;User ID=sa;Password=123456789!_Asdf;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir DbModels
```

This works in a not default directory.
```
dotnet tool install --global dotnet-ef
dotnet ef dbcontext scaffold "Server=127.0.0.1;Database=AIChecker;User ID=sa;Password=123456789!_Asdf;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o DbModels
```

Send request to LMStudio

```
.\AIChecker.exe sendToLMS -m "Schreib mir ein Gedicht" -s "Du achtest darauf, dass ich alles reimt" -r "Requesttime check: | model: Phi-3.5-mini-instruct" -c 5
```
### Installing packages

```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

### Configure database connection
```yml
"ConnectionStrings": {
  "PostgresConnection": "Host=localhost;Port=5432;Database=mydatabase;Username=myuser;Password=mypassword"
}
```

###  Register ApplicationDbContext in the DI Container
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
```

### Run Migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
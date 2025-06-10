using System.Text;
using API.Middlewares;
using API.Models;
using API.Utils.DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();

        builder.Services.AddTransient<SecurityService>();
        
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

        builder.Services.AddAuthorization();
        
        var app = builder.Build();
        
        app.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }));

        // Enable routing and controllers
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            SeedData(dbContext);
        }
        
        // Custom Middlewares
        app.UseMiddleware<RequestResponseLoggingMiddleware>();
        app.UseMiddleware<ErrorHandlingMiddleware>(); // Order matters

        app.Run();
    }
    
    private static void SeedData(ApplicationDbContext context)
    {
        // Ensure the company exists first, or create one
        var company = context.Companies.FirstOrDefault(c => c.Name == "Alice's store") ?? new Company() { Name = "Alice's store" };

        if (!context.Companies.Any(c => c.Name == "Alice's store"))
        {
            context.Companies.Add(company);
            context.SaveChanges();
            
        }

        if (!context.Users.Any(u => u.Email == "alice@example.com"))
        {
            User alice = new User(){Name = "Alice", Email = "alice@example.com", Password = "password", Company = company};
            context.Users.Add(alice);
            context.SaveChanges();
        }
    }
}

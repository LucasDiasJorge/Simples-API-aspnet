using API.Middlewares;

namespace API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        builder.Services.AddAuthorization();
        
        var app = builder.Build();

        app.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }));

        // Enable routing and controllers
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();
        
        // Custom Middlewares
        app.UseMiddleware<RequestResponseLoggingMiddleware>();


        app.Run();
    }
}

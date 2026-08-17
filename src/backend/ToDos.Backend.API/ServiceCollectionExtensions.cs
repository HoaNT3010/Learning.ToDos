using Microsoft.EntityFrameworkCore;
using ToDos.Backend.API.Data;
using ToDos.Backend.API.Middlewares;

namespace ToDos.Backend.API;

public static class ServicesCollectionExtensions
{
    public const string ConnectionStringName = "Database";

    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddOpenApi();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddDatabase(configuration);

        return services;
    }

    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(ConnectionStringName));
        });

        return services;
    }
}

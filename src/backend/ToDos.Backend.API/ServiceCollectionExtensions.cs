using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ToDos.Backend.API.Data;
using ToDos.Backend.API.Middlewares;
using ToDos.Backend.API.Services;

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

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<IToDoItemService, ToDoItemService>();
        services.AddControllers();

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

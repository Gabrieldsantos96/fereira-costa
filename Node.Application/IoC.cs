using Microsoft.Extensions.DependencyInjection;

namespace Node.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddProblemDetails();
        return services;
    }
}
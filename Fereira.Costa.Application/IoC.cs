using Microsoft.Extensions.DependencyInjection;

namespace Fereira.Costa.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddProblemDetails();
        return services;
    }
}
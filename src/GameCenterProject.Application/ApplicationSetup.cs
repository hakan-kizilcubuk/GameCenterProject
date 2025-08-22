using GameCenterProject.Application.Abstractions;
using GameCenterProject.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameCenterProject.Application;

public static class ApplicationSetup
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ILibraryService, LibraryService>();
        return services;
    }
}

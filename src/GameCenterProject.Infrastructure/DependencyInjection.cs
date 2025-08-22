using GameCenterProject.Infrastructure.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using GameCenterProject.Infrastructure.Persistence;
using GameCenterProject.Infrastructure.Concrete;

namespace GameCenterProject.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string connectionName = "Default")
        {
            var cs = configuration.GetConnectionString(connectionName)
            ?? "Server=localhost;Database=GameCenter;Trusted_Connection=True;";

            services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(cs, sql =>
            {
                sql.EnableRetryOnFailure();
                sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
            }));

        services.AddScoped<IGameRepository, EfGameRepository>();
        services.AddScoped<IGameEditionRepository, EfGameEditionRepository>();
        services.AddScoped<ICartRepository, EfCartRepository>();
        services.AddScoped<ILibraryRepository, EfLibraryRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        return services;
        }
    }
}
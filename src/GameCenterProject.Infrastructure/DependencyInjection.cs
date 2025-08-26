using GameCenterProject.Infrastructure.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using GameCenterProject.Infrastructure.Persistence;
using GameCenterProject.Infrastructure.Concrete;
using GameCenterProject.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace GameCenterProject.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string connectionName = "Default")
        {
            var cs = configuration.GetConnectionString(connectionName)
            ?? "Server=localhost;Database=GameCenter;Trusted_Connection=True;Encrypt=False;";

            services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(cs, sql =>
            {
                sql.EnableRetryOnFailure();
                sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
            }));

            // Identity
            services.AddIdentityCore<ApplicationUser>(o =>
            {
                o.User.RequireUniqueEmail = true;
                o.Password.RequiredLength = 6;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<AppDbContext>();

            // JWT
            var jwtKey = configuration["Jwt:Key"] ?? "dev-secret-change-me-32chars-minimum----------------";
            var issuer = configuration["Jwt:Issuer"] ?? "GameCenter";
            var audience = configuration["Jwt:Audience"] ?? "GameCenter.Frontend";

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                });

            services.AddAuthorization();

            services.AddScoped<IGameRepository, EfGameRepository>();
            services.AddScoped<IGameEditionRepository, EfGameEditionRepository>();
            services.AddScoped<ICartRepository, EfCartRepository>();
            services.AddScoped<ILibraryRepository, EfLibraryRepository>();
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            return services;
        }
    }
}
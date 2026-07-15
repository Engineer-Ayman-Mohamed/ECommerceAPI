using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using ECommerceAPI.Domain.Interfaces;
using ECommerceAPI.Domain.Interfaces.Services;
using ECommerceAPI.Infrastructure.Data;
using ECommerceAPI.Infrastructure.Repositories;
using ECommerceAPI.Infrastructure.Services;

namespace ECommerceAPI.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        var redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!);
        services.AddSingleton<IConnectionMultiplexer>(redis);

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBasketRepository, BasketRepository>();

        services.Configure<CloudinarySettings>(options =>
        {
            options.CloudName = configuration["Cloudinary:CloudName"] ?? string.Empty;
            options.ApiKey = configuration["Cloudinary:ApiKey"] ?? string.Empty;
            options.ApiSecret = configuration["Cloudinary:ApiSecret"] ?? string.Empty;
        });
        services.AddScoped<IPictureService, PictureService>();
        
        return services;
    }
}

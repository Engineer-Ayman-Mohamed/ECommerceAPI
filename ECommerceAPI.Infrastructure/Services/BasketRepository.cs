using System.Text.Json;
using StackExchange.Redis;
using ECommerceAPI.Domain.Entities.Basket;
using ECommerceAPI.Domain.Interfaces;

namespace ECommerceAPI.Infrastructure.Services;

public class BasketRepository : IBasketRepository
{
    private readonly IDatabase _database;

    public BasketRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<CustomerBasket?> GetBasketAsync(string basketId)
    {
        var data = await _database.StringGetAsync(basketId);
        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data.ToString());
    }

    public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
    {
        var created = await _database.StringSetAsync(
            basket.Id,
            JsonSerializer.Serialize(basket),
            TimeSpan.FromDays(30));

        if (!created) return null;

        return await GetBasketAsync(basket.Id);
    }

    public async Task DeleteBasketAsync(string basketId)
    {
        await _database.KeyDeleteAsync(basketId);
    }
}

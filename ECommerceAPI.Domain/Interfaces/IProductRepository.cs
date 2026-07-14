using Ardalis.Specification;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces.Services;

namespace ECommerceAPI.Domain.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<Product?> GetByIdWithDetailsAsync(int id);
    Task<IReadOnlyList<Product>> GetAllWithDetailsAsync();
}

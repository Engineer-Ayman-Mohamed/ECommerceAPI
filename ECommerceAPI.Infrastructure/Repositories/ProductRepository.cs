using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using ECommerceAPI.Infrastructure.Data;

namespace ECommerceAPI.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context) { }

    public async Task<Product?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Products
            .Include(p => p.ProductBrand)
            .Include(p => p.ProductType)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyList<Product>> GetAllWithDetailsAsync()
    {
        return await _context.Products
            .Include(p => p.ProductBrand)
            .Include(p => p.ProductType)
            .ToListAsync();
    }
}

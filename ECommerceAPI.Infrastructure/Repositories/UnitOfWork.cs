using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using ECommerceAPI.Infrastructure.Data;

namespace ECommerceAPI.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<T> Repository<T>() where T : BaseEntity
    {
        var type = typeof(T);

        if (!_repositories.ContainsKey(type))
        {
            var repository = new GenericRepository<T>(_context);
            _repositories[type] = repository;
        }

        return (IGenericRepository<T>)_repositories[type];
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

namespace ECommerceAPI.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T> Repository<T>() where T : Entities.BaseEntity;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

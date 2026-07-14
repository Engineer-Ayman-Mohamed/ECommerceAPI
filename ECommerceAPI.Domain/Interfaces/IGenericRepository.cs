using Ardalis.Specification;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Domain.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    Task<int> CountAsync(ISpecification<T> spec);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}

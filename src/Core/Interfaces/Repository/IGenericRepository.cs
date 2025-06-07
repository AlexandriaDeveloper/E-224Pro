using System;
using Core.Interfaces.Specification;

namespace Core.Interfaces.Repository;

public interface IGenericRepository<T>
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default);
    Task AddRange2Async(List<T> entities, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateRangeAsync(List<T> entities, CancellationToken cancellationToken = default);
    void Delete(T entity, CancellationToken cancellationToken = default);
    Task<T?> GetById(int id, bool tracking = true, CancellationToken cancellationToken = default);
    Task<List<T?>> GetAll(ISpecification<T>? specification, CancellationToken cancellationToken = default);
    Task<int?> CountAsync(ISpecification<T>? specification, CancellationToken cancellationToken = default);

    IQueryable<T> GetQueryable(ISpecification<T>? specification = null, CancellationToken cancellationToken = default);


    Task<bool> ExistsAsync(ISpecification<T>? specification, CancellationToken cancellationToken = default);
    void RemoveRange(IEnumerable<T> entities, CancellationToken cancellationToken = default);


}

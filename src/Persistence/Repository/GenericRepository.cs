using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Core.Models;
using Core.Interfaces.Repository;
using Core.Interfaces.Specification;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Persistence.Specification;

namespace Persistence.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _accessor;

    public GenericRepository(ApplicationDbContext context, IHttpContextAccessor accessor)
    {
        _context = context;
        _accessor = accessor;
    }
    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.Id = entity.Id == 0 ? GetMaxId() + 1 : entity.Id; // Ensure Id is set if not already
        entity.CreatedDate = DateTime.Now;
        entity.CreatedBy = GetCurrentUserId() ?? "Anonymous";
        await _context.Set<T>().AddAsync(entity);
    }


    public async Task AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default)
    {
        // int MaxId = GetMaxId();

        foreach (var entity in entities)
        {
            // entity.Id = MaxId + 1;

            entity.CreatedDate = DateTime.Now;
            entity.CreatedBy = GetCurrentUserId() ?? "Anonymous";

            // MaxId++;

        }
        await _context.Set<T>().AddRangeAsync(entities);
    }
    public async Task AddRange2Async(List<T> entities, CancellationToken cancellationToken = default)
    {
        int MaxId = GetMaxId();
        foreach (var item in entities)
        {
            item.Id = MaxId + 1;
            item.CreatedDate = DateTime.Now;
            item.CreatedBy = GetCurrentUserId() ?? "Anonymous";
            MaxId++;
        }

        await _context.Set<T>().AddRangeAsync(entities);


    }

    protected string? GetCurrentUserId()
    {
        string? user = _accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return user != null ? user : null;
    }

    // public Task<int> CountAsync(ISpecification<T>? specification = null, CancellationToken cancellationToken = default)
    // {
    //     return _context.Set<T>().CountAsync(cancellationToken); ;
    // }

    public void Delete(T entity, CancellationToken cancellationToken = default)
    {

        _context.Set<T>().Remove(entity);
    }

    public async Task<List<T?>> GetAll(ISpecification<T>? specification = null, CancellationToken cancellationToken = default)
    {
        return specification is null
            ? await _context.Set<T>().ToListAsync()
            : await ApplySpecification(specification).ToListAsync();
    }

    public async Task<T?> GetById(int id, bool tracking = true, CancellationToken cancellationToken = default)
    {
        if (tracking == false)
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {

        // _context.Entry(entity).State = EntityState.Modified;
        await Task.FromResult(_context.Set<T>().Update(entity));
    }

    public IQueryable<T> GetQueryable(ISpecification<T>? specification = null, CancellationToken cancellationToken = default)
    {
        if (specification is null)
            return _context.Set<T>().AsQueryable();
        return ApplySpecification(specification).AsQueryable();

    }
    public async Task<int?> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
    {
        if (spec is null)
            return await _context.Set<T>().CountAsync(cancellationToken);

        return await ApplySpecification(spec).CountAsync();

    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }

    private int GetMaxId()
    {

        if (_context.Set<T>().AsNoTracking().Count() == 0)
        {
            return 0;  // No records found, return 0 as maxId.  Replace with your default value.  For example, if the ID is auto-incrementing, consider 1.  If it's a non-auto-incrementing key, consider 0.  If you're not using an ID, you can remove this method entirely.  It's a good practice to have a default value for your ID.  For example, if you're using an integer, consider -1.  If you're using a string, consider "00000000000000000000000000000000".  If you're using a GUID, consider Guid.Empty.  If you're using a timestamp, consider DateTime.MinValue.  If you're using a custom ID, consider a unique identifier.  If you're using a composite key, consider a combination
        }
        return _context.Set<T>().Max(x => x.Id);
    }

    public Task UpdateRangeAsync(List<T> entities, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().UpdateRange(entities);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(ISpecification<T>? specification, CancellationToken cancellationToken = default)
    {
        if (specification is null)
            return await _context.Set<T>().AnyAsync(cancellationToken);
        return await ApplySpecification(specification).AnyAsync(cancellationToken);
    }

    public void RemoveRange(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().RemoveRange(entities);
    }
}

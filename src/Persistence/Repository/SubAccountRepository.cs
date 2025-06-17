
using Core.Interfaces.Repository;
using Core.Interfaces.Specification;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository;

public class SubAccountRepository : GenericRepository<SubAccount>, ISubAccountRepository
{
    private readonly ApplicationDbContext _context;


    public SubAccountRepository(ApplicationDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
    {
        this._context = context;

    }

    public async Task<List<SubAccount>> GetSubAccountsByAccountId(int value, CancellationToken cancellationToken, ISpecification<SubAccount>? spec = null)
    {
        return await this.GetQueryable(spec).Where(x => x.AccountId == value).ToListAsync(cancellationToken);
    }
}
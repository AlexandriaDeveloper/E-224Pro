
using Core.Interfaces.Repository;
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

    public async Task<List<SubAccount>> GetSubAccountsByAccountId(int value, CancellationToken cancellationToken)
    {
        return await _context.SubAccounts.Where(x => x.AccountId == value).ToListAsync(cancellationToken);
    }
}
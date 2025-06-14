
using Core.Interfaces.Repository;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository;

public class FormRepository : GenericRepository<Form>, IFormRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _accessor;

    public FormRepository(ApplicationDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
    {
        this._context = context;
        this._accessor = accessor;
    }

    public Task<List<Form>> GetSubsidaryDailyFormsByDailyIdAndSubsidaryId(int id, int dailyId, CancellationToken cancellationToken)
    {

        return _context.Forms.Where(x => x.DailyId == dailyId)
        .Include(x => x.FormDetails.Where(x => x.AccountId == id))
        .ToListAsync(cancellationToken);
    }
}

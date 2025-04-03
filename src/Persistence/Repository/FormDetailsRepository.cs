
using Core.Interfaces.Repository;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository;

public class FormDetailsRepository : GenericRepository<FormDetails>, IFormDetailsRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _accessor;

    public FormDetailsRepository(ApplicationDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
    {
        this._context = context;
        this._accessor = accessor;
    }

    public async Task<List<FormDetails?>> GetByFormId(int formId)
    {
        return await _context.FormDetails.Where(x => x.FormId == formId).Include(x => x.Account).ToListAsync();
    }
}
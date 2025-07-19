
using Core.Interfaces.Repository;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository;

public class SubsidiaryJournalRepository : GenericRepository<SubsidiaryJournal>, ISubsidiaryJournalRepository
{
    private readonly ApplicationDbContext _context;

    public SubsidiaryJournalRepository(ApplicationDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
    {
        _context = context;
    }

    public Task<List<SubsidiaryJournal>> GetByFormDetailsId(int formDetailsId)
    {
        return _context.SubsidiaryJournals
            .Where(x => x.FormDetailsId == formDetailsId)
           .ToListAsync();
    }
}
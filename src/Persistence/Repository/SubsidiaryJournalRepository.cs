
using Core.Interfaces.Repository;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Persistence.Repository;

public class SubsidiaryJournalRepository : GenericRepository<SubsidiaryJournal>, ISubsidiaryJournalRepository
{
    public SubsidiaryJournalRepository(ApplicationDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
    {
    }
}
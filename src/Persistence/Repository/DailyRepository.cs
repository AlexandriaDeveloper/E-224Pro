
using Core.Interfaces.Repository;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Persistence.Repository;

public class DailyRepository : GenericRepository<Daily>, IDailyRepository
{
    public DailyRepository(ApplicationDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
    {
    }
}

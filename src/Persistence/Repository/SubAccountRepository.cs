
using Core.Interfaces.Repository;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Persistence.Repository;

public class SubAccountRepository : GenericRepository<SubAccount>, ISubAccountRepository
{
    public SubAccountRepository(ApplicationDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
    {
    }
}
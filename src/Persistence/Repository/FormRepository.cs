
using Core.Interfaces.Repository;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Http;

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


}


using Core.Interfaces.Repository;
using Infrastructure;

namespace Persistence.Repository;

public class UOW : IUow
{
    private readonly ApplicationDbContext context;
    public UOW(ApplicationDbContext context)
    {
        this.context = context;
    }
    public Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return context.SaveChangesAsync(cancellationToken); ///
           // return context.SaveChangesAsyncWithHistory(); ///
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }
}

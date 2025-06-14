using Core.Models;

namespace Core.Interfaces.Repository;

public interface ISubAccountRepository : IGenericRepository<SubAccount>
{
    Task<List<SubAccount>> GetSubAccountsByAccountId(int value, CancellationToken cancellationToken);
}

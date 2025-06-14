using Core.Models;

namespace Core.Interfaces.Repository;

public interface IFormRepository : IGenericRepository<Form>
{
    Task<List<Form>> GetSubsidaryDailyFormsByDailyIdAndSubsidaryId(int id, int dailyId, CancellationToken cancellationToken);
}

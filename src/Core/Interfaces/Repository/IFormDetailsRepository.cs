using Core.Models;

namespace Core.Interfaces.Repository;

public interface IFormDetailsRepository : IGenericRepository<FormDetails>
{
    Task<List<FormDetails?>> GetByFormId(int formId);
}
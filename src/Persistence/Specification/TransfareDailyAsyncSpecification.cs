using Core.Models;

namespace Persistence.Specification;

public class TransfareDailyAsyncSpecification : Specification<Daily>
{
    public TransfareDailyAsyncSpecification(int id) : base(x => x.Id == id)
    {

        AddInclude(x => x.Forms);







    }




}

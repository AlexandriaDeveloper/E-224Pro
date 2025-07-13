using Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace Persistence.Specification;

public class GetAccountSpecification : Specification<Account>
{

    public GetAccountSpecification(GetAccountRequest request)
    {
        if (request.Id.HasValue)
        {
            AddCriteries(x => x.Id == request.Id!.Value);
        }
        if (!string.IsNullOrEmpty(request.AccountName))
        {
            AddCriteries(x => x.AccountName == request.AccountName!);
        }
        if (request.Id.HasValue)
        {
            AddCriteries(x => x.Id == request.Id!);
        }



    }
}

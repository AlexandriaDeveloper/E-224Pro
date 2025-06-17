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
        if (!request.AccountName.IsNullOrEmpty())
        {
            AddCriteries(x => x.AccountName!.Contains(request.AccountName!));
        }
        if (!request.AccountNumber.IsNullOrEmpty())
        {
            AddCriteries(x => x.AccountNumber!.Contains(request.AccountNumber!));
        }

    }
}


using Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace Persistence.Specification;

public class GetFundsSpecifiction : Specification<Fund>
{
    public GetFundsSpecifiction(GetFundRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.FundName))
        {
            AddCriteries(x => x.FundName.Contains(request.FundName));
        }
        if (!request.FundCode.IsNullOrEmpty())
        {
            AddCriteries(x => x.FundCode.Contains(request.FundCode));
        }
        if (request.Id.HasValue)
        {
            AddCriteries(x => x.Id.Equals(request.Id.Value));
        }
        if (request.CollageId.HasValue)
        {
            AddCriteries(x => x.CollageId.Equals(request.CollageId.Value));
        }

    }
}



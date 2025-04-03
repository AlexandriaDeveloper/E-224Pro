using Core.Models;
using Microsoft.IdentityModel.Tokens;
using Shared.Contracts.FormRequests;
using Shared.Contracts.ReportRequest;

namespace Persistence.Specification;

public class GetReportyBySpecification : Specification<FormDetails>
{

    public GetReportyBySpecification(GetAccountsBalanceBy request)
    {
        AddInclude(x => x.Form!);
        AddInclude(x => x.Form!.Daily!);
        if (request.SpecificDate.HasValue)
        {
            AddCriteries(x => x.Form!.Daily!.DailyDate == request.SpecificDate);
        }

        if (request.ByMonth.HasValue)
        {
            AddCriteries(x => x.Form!.Daily!.DailyDate.Month == request.ByMonth);
        }

        if (!request.AccountItem.IsNullOrEmpty())
        {
            AddCriteries(x => x.Form!.Daily!.AccountItem == request.AccountItem);
        }

        if (request.StartDate.HasValue)
        {
            AddCriteries(x => x.Form!.Daily!.DailyDate >= request.StartDate);
        }
        if (request.EndDate.HasValue)
        {
            AddCriteries(x => x.Form!.Daily!.DailyDate <= request.EndDate);
        }
        if (request.CollageId.HasValue)
        {
            AddInclude(x => x.Form!.Collage!);
            AddCriteries(x => x.Form!.CollageId == request.CollageId);
        }

        if (request.FunId.HasValue)
        {
            AddInclude(x => x.Form!.Fund!);
            AddCriteries(x => x.Form!.FundId == request.FunId);
        }
        if (request.FormId.HasValue)
        {
            AddCriteries(x => x.FormId == request.FormId);
        }

        // if (!string.IsNullOrEmpty(request.FundName))
        // {
        //     AddInclude(x => x.Form!.Fund!);
        //     AddCriteries(x => x.Form!.Fund!.FundName == request.FundName);
        // }
        if (!string.IsNullOrEmpty(request.AccountType))
        {

            AddCriteries(x => x.Form!.Daily!.DailyType == request.AccountType);
        }


    }
}

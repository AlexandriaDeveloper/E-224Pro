using Azure.Core;
using Core.Constants;
using Core.Models;
using Shared.Constants.Enums;
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
        if (request.DailyId.HasValue)
        {
            AddInclude(x => x.Form!.Daily!);
            AddCriteries(x => x.Form!.Daily!.Id == request.DailyId);
        }

        if (request.ByMonth.HasValue && request.ByYear.HasValue)
        {
            AddCriteries(x => x.Form!.Daily!.DailyDate.Month == request.ByMonth && x.Form.Daily.DailyDate.Year == request.ByYear);
        }

        // if (!string.IsNullOrEmpty(request.AccountItem))
        // {
        //     AddCriteries(x => x.Form!.Daily!.AccountItem == request.AccountItem);
        // }

        if (request.StartDate.HasValue)
        {
            AddCriteries(x => x.Form!.Daily!.DailyDate >= request.StartDate);
        }
        if (request.EndDate.HasValue)
        {
            AddCriteries(x => x.Form!.Daily!.DailyDate <= request.EndDate);
        }
        if (request.EntryType.HasValue)
        {
            AddCriteries(x => x.Form!.EntryType == (EntryTypeEnum)request.EntryType.Value);
        }

        if (request.CollageId.HasValue)
        {
            AddInclude(x => x.Form!.Collage!);
            AddCriteries(x => x.Form!.CollageId == request.CollageId);
        }

        if (request.FundId.HasValue)
        {
            AddInclude(x => x.Form!.Fund!);
            AddCriteries(x => x.Form!.FundId == request.FundId);
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
        if (!string.IsNullOrEmpty(request.DailyType))
        {

            AddCriteries(x => x.Form!.Daily!.DailyType == request.DailyType);
        }


    }
}

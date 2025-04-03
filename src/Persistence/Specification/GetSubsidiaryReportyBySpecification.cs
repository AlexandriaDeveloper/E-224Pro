using Core.Models;
using Microsoft.IdentityModel.Tokens;
using Shared.Contracts.ReportRequest;

namespace Persistence.Specification;

public class GetSubsidiaryReportyBySpecification : Specification<SubsidiaryJournal>
{

    public GetSubsidiaryReportyBySpecification(GetSubSidiaryBalanceBy request)
    {
        AddInclude(x => x.FormDetails);
        AddInclude(x => x.FormDetails.Form!);
        AddInclude(x => x.FormDetails.Form!.Daily!);


        if (request.ByMonth.HasValue)
        {
            AddCriteries(x => x.FormDetails.Form!.Daily!.DailyDate.Month == request.ByMonth);
        }

        if (!request.AccountItem.IsNullOrEmpty())
        {
            AddCriteries(x => x.FormDetails.Form!.Daily!.AccountItem == request.AccountItem);
        }
        if (request.SpecificDate.HasValue)
        {
            AddCriteries(x => x.FormDetails.Form!.Daily!.DailyDate == request.SpecificDate);
        }

        if (request.StartDate.HasValue)
        {
            AddCriteries(x => x.FormDetails.Form!.Daily!.DailyDate >= request.StartDate);
        }
        if (request.EndDate.HasValue)
        {
            AddCriteries(x => x.FormDetails.Form!.Daily!.DailyDate <= request.EndDate);
        }
        if (request.CollageId.HasValue)
        {
            AddInclude(x => x.FormDetails.Form!.Collage!);
            AddCriteries(x => x.FormDetails.Form!.CollageId == request.CollageId);
        }

        if (request.FundId.HasValue)
        {
            AddInclude(x => x.FormDetails.Form!.Fund!);
            AddCriteries(x => x.FormDetails.Form!.FundId == request.FundId.Value);
        }
        if (request.FormId.HasValue)
        {

            AddCriteries(x => x.FormDetails.FormId == request.FormId.Value);
        }
        if (request.FormDetailsId.HasValue)
        {
            AddCriteries(x => x.FormDetailsId == request.FormDetailsId);
        }
        if (request.SubAccountId.HasValue)
        {
            AddCriteries(x => x.SubAccountId == request.SubAccountId);
        }


        // if (!string.IsNullOrEmpty(request.FundName))
        // {
        //     AddInclude(x => x.Form!.Fund!);
        //     AddCriteries(x => x.Form!.Fund!.FundName == request.FundName);
        // }
        if (!string.IsNullOrEmpty(request.AccountType))
        {

            AddCriteries(x => x.FormDetails.Form!.Daily!.DailyType == request.AccountType);
        }


    }
}
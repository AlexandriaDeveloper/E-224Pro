using Core.Models;
using Microsoft.IdentityModel.Tokens;
using Persistence.Specification;
using Shared.Contracts.ReportRequest;

public class GetSubsidiaryJournalsSpecification : Specification<SubsidiaryJournal>
{
    public GetSubsidiaryJournalsSpecification(GetSubsidiaryJournalsRequest request)
    {


        AddInclude(x => x.Collage!);
        AddInclude(x => x.FormDetails!);
        AddInclude(x => x.Fund!);
        AddInclude(x => x.SubAccount!);

        AddInclude(x => x.FormDetails!.Form!.Daily!);
        if (request.FundId.HasValue)
        {
            AddCriteries(x => x.FundId == request.FundId.Value);
        }
        if (request.CollageId.HasValue)
        {
            AddCriteries(x => x.CollageId == request.CollageId.Value);
        }
        if (request.FormDetailsId.HasValue)
        {
            AddCriteries(x => x.FormDetailsId == request.FormDetailsId.Value);
        }
        if (request.SubAccountId.HasValue)
        {
            AddCriteries(x => x.SubAccountId == request.SubAccountId.Value);

        }
        if (request.Id.HasValue)
        {
            AddCriteries(x => x.Id == request.Id.Value);

        }
        if (!string.IsNullOrEmpty(request.AccountItem))
        {
            AddCriteries(x => x.AccountItem.Equals(request.AccountItem));
        }
        if (!string.IsNullOrEmpty(request.AccountType))
        {
            AddCriteries(x => x.AccountType.Equals(request.AccountType));
        }
        if (request.DateFrom.HasValue)
        {

            AddCriteries(x => x.FormDetails.Form!.Daily!.DailyDate >= request.DateFrom.Value);
        }
        if (request.DateTo.HasValue)
        {

            AddCriteries(x => x.FormDetails!.Form!.Daily!.DailyDate <= request.DateTo.Value);
        }





    }


}
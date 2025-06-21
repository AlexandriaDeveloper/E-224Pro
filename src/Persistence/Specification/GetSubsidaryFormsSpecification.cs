using Core.Models;
using Microsoft.IdentityModel.Tokens;
using Persistence.Specification;

public class GetSubsidaryFormsSpecification : Specification<Form>
{
    public GetSubsidaryFormsSpecification(GetSubsidiaryFormsByDailyIdRequest request)
    {
        // AddInclude(x => x.Daily);
        // AddInclude(x => x.FormDetails!);
        //then include subsidiary journal
        //  AddInclude(x => x.FormDetails!.Select(x => x.SubsidiaryJournals)!);





        if (request.Id.HasValue)
        {
            AddCriteries(x => x.Id == request.Id.Value);
        }
        if (request.FormDetailsId.HasValue)
        {
            AddCriteries(x => x.Id == request.FormDetailsId.Value);
        }
        if (request.DailyId.HasValue)
        {

            AddCriteries(x => x.DailyId! == request.DailyId.Value);
        }
        // if (request.SubAccountId.HasValue)
        // {
        //     AddCriteries(x => x.FormDetails.Select(x => x.AccountId).Contains(request.SubAccountId.Value));
        // }
        if (request.CollageId.HasValue)
        {

            AddCriteries(x => x.CollageId == request.CollageId.Value);
        }
        if (request.FundId.HasValue)
        {
            AddCriteries(x => x.FundId! == request.FundId.Value);
        }
        if (!request.Num55.IsNullOrEmpty())
        {
            AddCriteries(x => x.Num55! == request.Num55);

        }
        if (!request.Num224.IsNullOrEmpty())
        {
            AddCriteries(x => x.Num224! == request.Num224);

        }

        AddOrderByDescending(x => x.Id);
        if (request.PageIndex.HasValue)
        {
            ApplyPaging(request.PageIndex.Value, request.PageSize!.Value);
        }


    }

}
using Core.Models;
using Microsoft.IdentityModel.Tokens;
using Shared.DTOs.FormDtos;

namespace Persistence.Specification;

public class GetSubsidaryDailyBySpecification : Specification<FormDetails>
{

    public GetSubsidaryDailyBySpecification(GetSubsidartDailyRequest request)
    {
        AddInclude(x => x.SubsidiaryJournals!);
        AddInclude(x => x.Form!);
        AddInclude(x => x.Form!.Daily!);
        AddInclude(x => x.Form!.Collage!);
        AddInclude(x => x.Form!.Fund!);
        AddInclude(x => x.Account);
        AddInclude(x => x.Account.SubAccounts);

        if (request.AccountId.HasValue)
        {
            AddCriteries(x => x.AccountId == request.AccountId);
        }

        if (request.CollageId.HasValue)
        {
            AddCriteries(x => x.Form.CollageId! == request.CollageId);
        }
        if (request.FundId.HasValue)
        {
            AddCriteries(x => x.Form!.FundId! == request.FundId);
        }

        if (request.DailyId.HasValue)
        {

            AddCriteries(x => x.Form!.DailyId == request.DailyId.Value);
        }
        if (!request.DailyType.IsNullOrEmpty())
        {

            AddCriteries(x => x.Form.DailyId == request.DailyId.Value);
        }
        if (request.AccountId.HasValue)
        {

            AddCriteries(x => x.AccountId == request.AccountId.Value);
        }

        if (request.StartDate.HasValue)
        {
            AddCriteries(x => x.Form!.Daily!.DailyDate >= request.StartDate);
        }

        if (request.EndDate.HasValue)
        {
            AddCriteries(x => x.Form!.Daily!.DailyDate <= request.EndDate);
        }

        if (!request.Num55.IsNullOrEmpty())
        {
            AddCriteries(x => x.Form!.Num55.Contains(request.Num55));
        }
        if (!request.Num224.IsNullOrEmpty())
        {
            AddCriteries(x => x.Form!.Num224.Contains(request.Num224));
        }



    }
}